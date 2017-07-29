using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Replays;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsClient : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LeaderboardsClient));
        private static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        private static readonly RetryPolicy<SteamClientTransientErrorDetectionStrategy> RetryPolicy = SteamClientTransientErrorDetectionStrategy.Create(RetryStrategy);

        #region Initialization

        public LeaderboardsClient(ILeaderboardsHttpClient httpClient,
            ILeaderboardsSqlClient sqlClient,
            ApiClient apiClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.sqlClient = sqlClient ?? throw new ArgumentNullException(nameof(sqlClient));
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        #endregion

        #region Fields

        private readonly ILeaderboardsHttpClient httpClient;
        private readonly ILeaderboardsSqlClient sqlClient;
        private readonly ApiClient apiClient;

        #endregion

        #region Leaderboards and Entries

        public async Task UpdateLeaderboardsAsync(CancellationToken cancellationToken)
        {
            using (new UpdateNotifier(Log, "leaderboards"))
            {
                var production = (from m in await httpClient.GetLeaderboardHeadersAsync(cancellationToken).ConfigureAwait(false)
                                  where m.IsProduction && !m.IsCooperative
                                  select m).ToList();

                if (cancellationToken.IsCancellationRequested) { return; }

                var headers = new List<LeaderboardHeader>();

                using (var file = File.OpenText("leaderboard-headers.json"))
                {
                    var serializer = new JsonSerializer();
                    var primaries_headers = ((Headers)serializer.Deserialize(file, typeof(Headers))).leaderboards;

                    var primaries = from m in production
                                    join o in primaries_headers on m.LeaderboardId equals o.id
                                    select m;
                    headers.AddRange(primaries);
                }

                var dailies = production.Where(m => m.Date != null);
                headers.AddRange(dailies);

                var leaderboardTasks = new List<Task<Leaderboard>>();

                Leaderboard[] leaderboards;
                using (var download = new DownloadNotifier(Log, "leaderboards"))
                {
                    foreach (var header in headers)
                    {
                        var leaderboard = httpClient.GetLeaderboardAsync(header, download.Progress, cancellationToken);
                        leaderboardTasks.Add(leaderboard);
                    }

                    leaderboards = await Task.WhenAll(leaderboardTasks).ConfigureAwait(false);
                }

                if (cancellationToken.IsCancellationRequested) { return; }

                await sqlClient.SaveChangesAsync(leaderboards, cancellationToken).ConfigureAwait(false);

                var entries = leaderboards.SelectMany(e => e.Entries).ToList();

                var players = entries.Select(e => e.SteamId)
                    .Distinct()
                    .Select(s => new Player { SteamId = s });
                await sqlClient.SaveChangesAsync(players, false, cancellationToken).ConfigureAwait(false);

                var replayIds = new HashSet<long>(from e in entries
                                                  where e.ReplayId != null
                                                  select e.ReplayId.Value);
                var replays = from e in replayIds
                              select new Replay { ReplayId = e };
                await sqlClient.SaveChangesAsync(replays, false, cancellationToken).ConfigureAwait(false);

                await sqlClient.SaveChangesAsync(entries).ConfigureAwait(false);
            }
        }

        public async Task UpdateLeaderboardsAsync(LeaderboardsSteamClient steamClient, CancellationToken cancellationToken)
        {
            using (new UpdateNotifier(Log, "leaderboards"))
            {
                var headers = new List<Header>();

                using (var file = File.OpenText("leaderboard-headers.json"))
                {
                    var serializer = new JsonSerializer();
                    var primaries_headers = ((Headers)serializer.Deserialize(file, typeof(Headers))).leaderboards;
                    headers.AddRange(primaries_headers.Where(h => h.id > 0));
                }

                var leaderboardTasks = new List<Task<Leaderboard>>();

                Leaderboard[] leaderboards;
                using (var download = new DownloadNotifier(Log, "leaderboards"))
                {
                    steamClient.Progress = download.Progress;

                    foreach (var header in headers)
                    {
                        var leaderboardTask = RetryPolicy.ExecuteAsync(() => steamClient.GetLeaderboardAsync(header), cancellationToken);
                        leaderboardTasks.Add(leaderboardTask);
                    }

                    leaderboards = await Task.WhenAll(leaderboardTasks).ConfigureAwait(false);
                }

                if (cancellationToken.IsCancellationRequested) { return; }

                await sqlClient.SaveChangesAsync(leaderboards, cancellationToken).ConfigureAwait(false);

                var entries = leaderboards.SelectMany(e => e.Entries).ToList();

                var players = entries.Select(e => e.SteamId)
                    .Distinct()
                    .Select(s => new Player { SteamId = s });
                await sqlClient.SaveChangesAsync(players, false, cancellationToken).ConfigureAwait(false);

                var replayIds = new HashSet<long>(from e in entries
                                                  where e.ReplayId != null
                                                  select e.ReplayId.Value);
                var replays = from e in replayIds
                              select new Replay { ReplayId = e };
                await sqlClient.SaveChangesAsync(replays, false, cancellationToken).ConfigureAwait(false);

                await sqlClient.SaveChangesAsync(entries).ConfigureAwait(false);
            }
        }

        public async Task UpdateDailyLeaderboardsAsync(LeaderboardsSteamClient steamClient, CancellationToken cancellationToken)
        {
            using (new UpdateNotifier(Log, "daily leaderboards"))
            {
                var headers = new List<DailyHeader>();

                using (var db = new LeaderboardsContext())
                {
                    var today = DateTime.Today;
                    var stale = await (from l in db.DailyLeaderboards
                                       orderby l.LastUpdate
                                       where l.Date != today
                                       select new DailyHeader
                                       {
                                           id = l.LeaderboardId,
                                           date = l.Date,
                                           product = l.ProductId == 1 ? "Amplified" : "Classic",
                                           production = l.IsProduction
                                       })
                                       .Take(98)
                                       .ToListAsync(cancellationToken);
                    headers.AddRange(stale);

                    IEnumerable<DailyHeader> todaysDailies =
                        await (from l in db.DailyLeaderboards
                               where l.Date == today
                               select new DailyHeader
                               {
                                   id = l.LeaderboardId,
                                   date = l.Date,
                                   product = l.ProductId == 1 ? "Amplified" : "Classic",
                                   production = l.IsProduction
                               })
                               .ToListAsync(cancellationToken);
                    if (todaysDailies.Count() != 2)
                    {
                        todaysDailies = await Task.WhenAll(
                            steamClient.GetHeaderAsync(today, "Classic", true),
                            steamClient.GetHeaderAsync(today, "Amplified", true)
                        ).ConfigureAwait(false);
                    }
                    headers.AddRange(todaysDailies);
                }

                var leaderboardTasks = new List<Task<DailyLeaderboard>>();
                DailyLeaderboard[] leaderboards;
                using (var download = new DownloadNotifier(Log, "daily leaderboards"))
                {
                    steamClient.Progress = download.Progress;

                    foreach (var header in headers)
                    {
                        var leaderboard = steamClient.GetLeaderboardAsync(header);
                        leaderboardTasks.Add(leaderboard);
                    }

                    leaderboards = await Task.WhenAll(leaderboardTasks).ConfigureAwait(false);
                }

                if (cancellationToken.IsCancellationRequested) { return; }

                await sqlClient.SaveChangesAsync(leaderboards, cancellationToken).ConfigureAwait(false);

                var entries = leaderboards.SelectMany(e => e.Entries).ToList();

                var players = entries.Select(e => e.SteamId)
                    .Distinct()
                    .Select(s => new Player { SteamId = s });
                await sqlClient.SaveChangesAsync(players, false, cancellationToken).ConfigureAwait(false);

                var replayIds = new HashSet<long>(from e in entries
                                                  where e.ReplayId != null
                                                  select e.ReplayId.Value);
                var replays = from e in replayIds
                              select new Replay { ReplayId = e };
                await sqlClient.SaveChangesAsync(replays, false, cancellationToken).ConfigureAwait(false);

                await sqlClient.SaveChangesAsync(entries, cancellationToken).ConfigureAwait(false);
            }
        }

        #endregion

        #region Players

        public async Task UpdatePlayersAsync(int limit, CancellationToken cancellationToken)
        {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException(nameof(limit));

            using (new UpdateNotifier(Log, "players"))
            {
                var steamIds = await apiClient.GetStaleSteamIdsAsync(limit, cancellationToken).ConfigureAwait(false);

                var players = await httpClient.GetPlayersAsync(steamIds, cancellationToken).ConfigureAwait(false);

                players = steamIds.GroupJoin(
                          players,
                          id => id,
                          p => p.SteamId,
                          (id, ps) =>
                          {
                              var p = ps.SingleOrDefault();
                              if (p != null)
                              {
                                  p.Exists = true;
                              }
                              else
                              {
                                  p = new Player
                                  {
                                      SteamId = id,
                                      Exists = false,
                                  };
                              }
                              p.LastUpdate = DateTime.UtcNow;
                              return p;
                          }).ToList();

                await apiClient.PostPlayersAsync(players, cancellationToken).ConfigureAwait(false);
            }
        }

        #endregion

        #region Replays

        private static readonly ReplaySerializer ReplaySerializer = new ReplaySerializer();

        public async Task UpdateReplaysAsync(int limit, CloudBlobDirectory directory, CancellationToken cancellationToken)
        {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException(nameof(limit));
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            using (new UpdateNotifier(Log, "replays"))
            {
                var missing = await apiClient.GetMissingReplayIdsAsync(limit, cancellationToken).ConfigureAwait(false);

                var replayContexts = await httpClient.GetReplaysAsync(missing, cancellationToken).ConfigureAwait(false);

                var replays = new List<Replay>(limit);
                var uploads = new List<Task<Uri>>(limit);

                foreach (var replayContext in replayContexts)
                {
                    var replay = new Replay
                    {
                        ReplayId = replayContext.UgcId,
                        ErrorCode = replayContext.ErrorCode,
                    };
                    replays.Add(replay);

                    if (replayContext.Data != null)
                    {
                        Task<Uri> upload;

                        try
                        {
                            var data = ReplaySerializer.Deserialize(replayContext.Data);

                            replay.Version = data.Header.Version;
                            replay.KilledBy = data.Header.KilledBy;
                            if (data.TryGetSeed(out int seed))
                            {
                                replay.Seed = seed;
                            }

                            var blob = directory.GetBlockBlobReference(replay.FileName);
                            blob.Properties.ContentType = "application/octet-stream";
                            blob.Properties.CacheControl = "max-age=604800"; // 1 week

                            upload = blob.UploadReplayDataAsync(data, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Failed to read replay from '{replayContext.DataUri}'.", ex);

                            var blob = directory.GetBlockBlobReference(replay.FileName);
                            blob.Properties.ContentType = "application/octet-stream";
                            blob.Properties.CacheControl = "max-age=604800"; // 1 week

                            // Upload unmodified data on failure
                            upload = blob.UploadRemoteReplayDataAsync(replayContext.Data, cancellationToken);
                        }

                        uploads.Add(upload);
                    }
                }

                while (uploads.Any())
                {
                    var upload = await Task.WhenAny(uploads).ConfigureAwait(false);
                    uploads.Remove(upload);

                    Uri uri = null;
                    try
                    {
                        uri = await upload.ConfigureAwait(false);

                        Log.Debug(uri);
                    }
                    catch (Exception ex)
                    {
                        var fileName = uri.Segments.Last();
                        var failed = replays.First(r => r.FileName == fileName);
                        replays.Remove(failed);

                        Log.Error($"Failed to upload {fileName}.", ex);
                    }
                }

                // TODO: Add rollback to stored replays in case this fails
                await apiClient.PostReplaysAsync(replays, cancellationToken).ConfigureAwait(false);
            }
        }

        #endregion

        #region IDisposable Members

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                httpClient.Dispose();
                apiClient.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}

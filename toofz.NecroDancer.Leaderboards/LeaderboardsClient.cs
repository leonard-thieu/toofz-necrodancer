using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.WindowsAzure.Storage.Blob;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Leaderboards.SteamWebApi;
using toofz.NecroDancer.Replays;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsClient : IDisposable
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(LeaderboardsClient));
        static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        static readonly RetryPolicy<SteamClientTransientErrorDetectionStrategy> RetryPolicy = SteamClientTransientErrorDetectionStrategy.Create(RetryStrategy);

        #region Initialization

        public LeaderboardsClient(
            ISteamWebApiClient httpClient,
            ILeaderboardsSqlClient sqlClient,
            IApiClient apiClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.sqlClient = sqlClient ?? throw new ArgumentNullException(nameof(sqlClient));
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        #endregion

        #region Fields

        readonly ISteamWebApiClient httpClient;
        readonly ILeaderboardsSqlClient sqlClient;
        readonly IApiClient apiClient;

        #endregion

        #region Leaderboards and Entries

        public async Task UpdateLeaderboardsAsync(
            LeaderboardsSteamClient steamClient,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (new UpdateNotifier(Log, "leaderboards"))
            {
                var headers = new List<LeaderboardHeader>();

                var leaderboardsService = new LeaderboardsService();
                var leaderboardHeaders = leaderboardsService.ReadLeaderboardHeaders("leaderboard-headers.json");
                headers.AddRange(leaderboardHeaders.Where(h => h.id > 0));

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

        public async Task UpdateDailyLeaderboardsAsync(
            LeaderboardsSteamClient steamClient,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (new UpdateNotifier(Log, "daily leaderboards"))
            {
                var headers = new List<DailyLeaderboardHeader>();

                var leaderboardsConnectionString = Util.GetEnvVar("LeaderboardsConnectionString");
                using (var db = new LeaderboardsContext(leaderboardsConnectionString))
                {
                    var leaderboardsService = new LeaderboardsService();
                    var categories = leaderboardsService.ReadCategories("leaderboard-categories.json");

                    var today = DateTime.Today;
                    var staleDailies = await (from l in db.DailyLeaderboards
                                              orderby l.LastUpdate
                                              where l.Date != today
                                              select new
                                              {
                                                  l.LeaderboardId,
                                                  l.Date,
                                                  l.ProductId,
                                                  l.IsProduction,
                                              })
                                              .Take(98)
                                              .ToListAsync(cancellationToken)
                                              .ConfigureAwait(false);
                    foreach (var staleDaily in staleDailies)
                    {
                        var header = new DailyLeaderboardHeader
                        {
                            id = staleDaily.LeaderboardId,
                            date = staleDaily.Date,
                            product = categories.GetItemName("products", staleDaily.ProductId),
                            production = staleDaily.IsProduction,
                        };
                        headers.Add(header);
                    }

                    var _todaysDailies =
                        await (from l in db.DailyLeaderboards
                               where l.Date == today
                               select new
                               {
                                   l.LeaderboardId,
                                   l.Date,
                                   l.ProductId,
                                   l.IsProduction,
                               })
                               .ToListAsync(cancellationToken)
                               .ConfigureAwait(false);
                    IEnumerable<DailyLeaderboardHeader> todaysDailies =
                        (from l in _todaysDailies
                         select new DailyLeaderboardHeader
                         {
                             id = l.LeaderboardId,
                             date = l.Date,
                             product = categories.GetItemName("products", l.ProductId),
                             production = l.IsProduction,
                         })
                         .ToList();
                    if (!todaysDailies.Any())
                    {
                        var requests = from p in categories["products"]
                                       select steamClient.GetDailyLeaderboardHeaderAsync(today, p.Key, true);
                        todaysDailies = await Task.WhenAll(requests).ConfigureAwait(false);
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
                        var leaderboard = steamClient.GetDailyLeaderboardAsync(header);
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

        public async Task UpdatePlayersAsync(
            int limit,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException(nameof(limit));

            using (new UpdateNotifier(Log, "players"))
            {
                var steamIds = (await apiClient.GetStaleSteamIdsAsync(limit, cancellationToken).ConfigureAwait(false)).ToList();

                var players = new ConcurrentBag<Player>();
                using (var download = new DownloadNotifier(Log, "players"))
                {
                    var requests = new List<Task>();
                    for (int i = 0; i < steamIds.Count; i += SteamWebApiClient.MaxPlayerSummariesPerRequest)
                    {
                        var ids = steamIds
                            .Skip(i)
                            .Take(SteamWebApiClient.MaxPlayerSummariesPerRequest);
                        var request = MapPlayers();
                        requests.Add(request);

                        async Task MapPlayers()
                        {
                            var playerSummaries = await httpClient
                                .GetPlayerSummariesAsync(ids, download.Progress, cancellationToken)
                                .ConfigureAwait(false);

                            foreach (var p in playerSummaries.Response.Players)
                            {
                                players.Add(new Player
                                {
                                    SteamId = p.SteamId,
                                    Name = p.PersonaName,
                                    Avatar = p.Avatar,
                                });
                            }

                        }
                    }
                    await Task.WhenAll(requests).ConfigureAwait(false);
                }

                Debug.Assert(!players.Any(p => p == null));

                // TODO: Document purpose.
                var playersIncludingNonExisting = steamIds.GroupJoin(
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
                    });

                await apiClient.PostPlayersAsync(playersIncludingNonExisting, cancellationToken).ConfigureAwait(false);
            }
        }

        #endregion

        #region Replays

        static readonly ReplaySerializer ReplaySerializer = new ReplaySerializer();

        public async Task UpdateReplaysAsync(
            int limit,
            CloudBlobDirectory directory,
            CancellationToken cancellationToken = default(CancellationToken))
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

        bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
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

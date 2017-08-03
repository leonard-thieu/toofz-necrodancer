using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        const int AppId = 247080;
        static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        static readonly RetryPolicy<SteamClientTransientErrorDetectionStrategy> RetryPolicy = SteamClientTransientErrorDetectionStrategy.Create(RetryStrategy);

        #region Initialization

        public LeaderboardsClient(
            ISteamWebApiClient steamWebApiClient,
            ILeaderboardsSqlClient sqlClient,
            IApiClient apiClient,
            IUgcHttpClient ugcHttpClient)
        {
            this.steamWebApiClient = steamWebApiClient ?? throw new ArgumentNullException(nameof(steamWebApiClient));
            this.sqlClient = sqlClient ?? throw new ArgumentNullException(nameof(sqlClient));
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.ugcHttpClient = ugcHttpClient ?? throw new ArgumentNullException(nameof(ugcHttpClient));
        }

        #endregion

        #region Fields

        readonly ISteamWebApiClient steamWebApiClient;
        readonly ILeaderboardsSqlClient sqlClient;
        readonly IApiClient apiClient;
        readonly IUgcHttpClient ugcHttpClient;

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
                            var playerSummaries = await steamWebApiClient
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

                var replays = new ConcurrentBag<Replay>();
                using (var download = new DownloadNotifier(Log, "replays"))
                {
                    var requests = new List<Task>();
                    foreach (var ugcId in missing)
                    {
                        var request = UpdateReplayAsync(ugcId);
                        requests.Add(request);
                    }
                    await Task.WhenAll(requests).ConfigureAwait(false);

                    async Task UpdateReplayAsync(long ugcId)
                    {
                        var replay = new Replay { ReplayId = ugcId };
                        replays.Add(replay);

                        try
                        {
                            var ugcFileDetails = await steamWebApiClient.GetUgcFileDetailsAsync(AppId, ugcId, download.Progress, cancellationToken).ConfigureAwait(false);
                            try
                            {
                                var ugcFile = await ugcHttpClient.GetUgcFileAsync(ugcFileDetails.Data.Url, download.Progress, cancellationToken).ConfigureAwait(false);
                                try
                                {
                                    var replayData = ReplaySerializer.Deserialize(ugcFile);
                                    replay.Version = replayData.Header.Version;
                                    replay.KilledBy = replayData.Header.KilledBy;
                                    if (replayData.TryGetSeed(out int seed))
                                    {
                                        replay.Seed = seed;
                                    }

                                    ugcFile.Dispose();
                                    ugcFile = new MemoryStream();
                                    ReplaySerializer.Serialize(ugcFile, replayData);
                                    ugcFile.Position = 0;
                                }
                                // TODO: Catch a more specific exception.
                                catch (Exception ex)
                                {
                                    Log.Error($"Unable to read replay from '{ugcFileDetails.Data.Url}'.", ex);
                                    // Upload unmodified data on failure
                                    ugcFile.Position = 0;
                                }
                                finally
                                {
                                    try
                                    {
                                        var blob = directory.GetBlockBlobReference(replay.FileName);
                                        blob.Properties.ContentType = "application/octet-stream";
                                        blob.Properties.CacheControl = "max-age=604800"; // 1 week

                                        await blob.UploadFromStreamAsync(ugcFile, cancellationToken).ConfigureAwait(false);

                                        Log.Debug(blob.Uri);
                                    }
                                    // TODO: Catch a more specific exception.
                                    catch (Exception ex)
                                    {
                                        Log.Error($"Failed to upload {replay.FileName}.", ex);
                                    }
                                }
                            }
                            catch (HttpRequestStatusException ex)
                            {
                                replay.ErrorCode = -(int)ex.StatusCode;
                            }
                        }
                        catch (HttpRequestStatusException ex)
                        {
                            replay.ErrorCode = (int)ex.StatusCode;
                        }
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
                steamWebApiClient.Dispose();
                apiClient.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Leaderboards.Services.Common;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;

namespace toofz.NecroDancer.Leaderboards.Services.LeaderboardUpdate
{
    sealed class WorkerRole : WorkerRoleBase<Settings>
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(WorkerRole));

        const uint AppId = 247080;

        public WorkerRole() : base("toofz Leaderboard Service") { }

        protected override string SettingsPath => "leaderboard-settings.json";

        protected override void OnStartOverride() { }

        protected override async Task RunAsyncOverride(CancellationToken cancellationToken)
        {
            var userName = Util.GetEnvVar("SteamUserName");
            var password = Util.GetEnvVar("SteamPassword");
            var leaderboardsConnectionString = Util.GetEnvVar("LeaderboardsConnectionString");

            var leaderboardsSqlClient = new LeaderboardsSqlClient(leaderboardsConnectionString);

            using (var steamClient = new SteamClientApiClient(userName, password))
            {
                await UpdateLeaderboardsAsync(steamClient, leaderboardsSqlClient, cancellationToken).ConfigureAwait(false);

                using (var db = new LeaderboardsContext(leaderboardsConnectionString))
                {
                    await UpdateDailyLeaderboardsAsync(steamClient, leaderboardsSqlClient, db, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        internal async Task UpdateLeaderboardsAsync(
            ISteamClientApiClient steamClient,
            ILeaderboardsSqlClient leaderboardsSqlClient,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (steamClient == null)
                throw new ArgumentNullException(nameof(steamClient), $"{nameof(steamClient)} is null.");
            if (leaderboardsSqlClient == null)
                throw new ArgumentNullException(nameof(leaderboardsSqlClient), $"{nameof(leaderboardsSqlClient)} is null.");

            using (new UpdateNotifier(Log, "leaderboards"))
            {
                var headers = new List<LeaderboardHeader>();

                var leaderboardHeaders = LeaderboardsResources.ReadLeaderboardHeaders("leaderboard-headers.min.json");
                headers.AddRange(leaderboardHeaders.Where(h => h.id > 0));

                var categories = LeaderboardsResources.ReadCategories("leaderboard-categories.min.json");

                Leaderboard[] leaderboards;
                using (var download = new DownloadNotifier(Log, "leaderboards"))
                {
                    steamClient.Progress = download.Progress;

                    var leaderboardTasks = new List<Task<Leaderboard>>();
                    foreach (var header in headers)
                    {
                        leaderboardTasks.Add(MapLeaderboardEntries());

                        async Task<Leaderboard> MapLeaderboardEntries()
                        {
                            var leaderboard = new Leaderboard
                            {
                                LeaderboardId = header.id,
                                CharacterId = categories.GetItemId("characters", header.character),
                                RunId = categories.GetItemId("runs", header.run),
                                LastUpdate = DateTime.UtcNow,
                            };

                            var response =
                                await steamClient.GetLeaderboardEntriesAsync(AppId, header.id, cancellationToken).ConfigureAwait(false);

                            leaderboard.EntriesCount = response.EntryCount;
                            var leaderboardEntries = response.Entries.Select(e =>
                            {
                                var entry = new Entry
                                {
                                    LeaderboardId = header.id,
                                    Rank = e.GlobalRank,
                                    SteamId = (long)(ulong)e.SteamID,
                                    Score = e.Score,
                                    Zone = e.Details[0],
                                    Level = e.Details[1],
                                };
                                var ugcId = (long)(ulong)e.UGCId;
                                switch (ugcId)
                                {
                                    case -1: entry.ReplayId = null; break;
                                    default: entry.ReplayId = ugcId; break;
                                }

                                return entry;
                            });
                            leaderboard.Entries.AddRange(leaderboardEntries);

                            return leaderboard;
                        }
                    }

                    leaderboards = await Task.WhenAll(leaderboardTasks).ConfigureAwait(false);
                }

                if (cancellationToken.IsCancellationRequested) { return; }

                await leaderboardsSqlClient.SaveChangesAsync(leaderboards, cancellationToken).ConfigureAwait(false);

                var entries = leaderboards.SelectMany(e => e.Entries).ToList();

                var players = entries.Select(e => e.SteamId)
                    .Distinct()
                    .Select(s => new Player { SteamId = s });
                await leaderboardsSqlClient.SaveChangesAsync(players, false, cancellationToken).ConfigureAwait(false);

                var replayIds = new HashSet<long>(from e in entries
                                                  where e.ReplayId != null
                                                  select e.ReplayId.Value);
                var replays = from e in replayIds
                              select new Replay { ReplayId = e };
                await leaderboardsSqlClient.SaveChangesAsync(replays, false, cancellationToken).ConfigureAwait(false);

                await leaderboardsSqlClient.SaveChangesAsync(entries).ConfigureAwait(false);
            }
        }

        internal async Task UpdateDailyLeaderboardsAsync(
            ISteamClientApiClient steamClient,
            ILeaderboardsSqlClient leaderboardsSqlClient,
            LeaderboardsContext db,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (steamClient == null)
                throw new ArgumentNullException(nameof(steamClient), $"{nameof(steamClient)} is null.");
            if (leaderboardsSqlClient == null)
                throw new ArgumentNullException(nameof(leaderboardsSqlClient), $"{nameof(leaderboardsSqlClient)} is null.");
            if (db == null)
                throw new ArgumentNullException(nameof(db), $"{nameof(db)} is null.");

            using (new UpdateNotifier(Log, "daily leaderboards"))
            {
                var headers = new List<DailyLeaderboardHeader>();

                IEnumerable<DailyLeaderboardHeader> todaysDailies;
                var categories = LeaderboardsResources.ReadCategories("leaderboard-categories.min.json");
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
                todaysDailies = (from l in _todaysDailies
                                 select new DailyLeaderboardHeader
                                 {
                                     id = l.LeaderboardId,
                                     date = l.Date,
                                     product = categories.GetItemName("products", l.ProductId),
                                     production = l.IsProduction,
                                 })
                                 .ToList();

                if (todaysDailies.Count() != categories.GetCategory("products").Count)
                {
                    var headerTasks = new List<Task<DailyLeaderboardHeader>>();
                    foreach (var p in categories["products"])
                    {
                        headerTasks.Add(GetDailyLeaderboardHeaderAsync());

                        async Task<DailyLeaderboardHeader> GetDailyLeaderboardHeaderAsync()
                        {
                            var isProduction = true;
                            var name = GetDailyLeaderboardName(p.Key, today, isProduction);
                            var leaderboard = await steamClient.FindLeaderboardAsync(AppId, name, cancellationToken).ConfigureAwait(false);

                            return new DailyLeaderboardHeader
                            {
                                id = leaderboard.ID,
                                date = today,
                                product = p.Key,
                                production = isProduction,
                            };
                        }
                    }
                    todaysDailies = await Task.WhenAll(headerTasks).ConfigureAwait(false);

                    string GetDailyLeaderboardName(string product, DateTime date, bool isProduction)
                    {
                        var tokens = new List<string>();

                        switch (product)
                        {
                            case "amplified": tokens.Add("DLC"); break;
                            case "classic": break;
                            default:
                                throw new ArgumentException($"'{product}' is not a valid product.");
                        }

                        tokens.Add(date.ToString("d/M/yyyy"));

                        var name = string.Join(" ", tokens);
                        if (isProduction) { name += "_PROD"; }

                        return name;
                    }
                }
                headers.AddRange(todaysDailies);

                var leaderboardTasks = new List<Task<DailyLeaderboard>>();
                DailyLeaderboard[] leaderboards;
                using (var download = new DownloadNotifier(Log, "daily leaderboards"))
                {
                    steamClient.Progress = download.Progress;

                    foreach (var header in headers)
                    {
                        leaderboardTasks.Add(MapLeaderboardEntries());

                        async Task<DailyLeaderboard> MapLeaderboardEntries()
                        {
                            var leaderboard = new DailyLeaderboard
                            {
                                LeaderboardId = header.id,
                                Date = header.date,
                                ProductId = categories.GetItemId("products", header.product),
                                IsProduction = header.production,
                                LastUpdate = DateTime.UtcNow,
                            };

                            var response =
                                await steamClient.GetLeaderboardEntriesAsync(AppId, header.id, cancellationToken).ConfigureAwait(false);

                            var leaderboardEntries = response.Entries.Select(e =>
                            {
                                var entry = new DailyEntry
                                {
                                    LeaderboardId = header.id,
                                    Rank = e.GlobalRank,
                                    SteamId = (long)(ulong)e.SteamID,
                                    Score = e.Score,
                                    Zone = e.Details[0],
                                    Level = e.Details[1],
                                };
                                var ugcId = (long)(ulong)e.UGCId;
                                switch (ugcId)
                                {
                                    case -1: entry.ReplayId = null; break;
                                    default: entry.ReplayId = ugcId; break;
                                }

                                return entry;
                            });
                            leaderboard.Entries.AddRange(leaderboardEntries);

                            return leaderboard;
                        }
                    }

                    leaderboards = await Task.WhenAll(leaderboardTasks).ConfigureAwait(false);
                }

                if (cancellationToken.IsCancellationRequested) { return; }

                await leaderboardsSqlClient.SaveChangesAsync(leaderboards, cancellationToken).ConfigureAwait(false);

                var entries = leaderboards.SelectMany(e => e.Entries).ToList();

                var players = entries.Select(e => e.SteamId)
                    .Distinct()
                    .Select(s => new Player { SteamId = s });
                await leaderboardsSqlClient.SaveChangesAsync(players, false, cancellationToken).ConfigureAwait(false);

                var replayIds = new HashSet<long>(from e in entries
                                                  where e.ReplayId != null
                                                  select e.ReplayId.Value);
                var replays = from e in replayIds
                              select new Replay { ReplayId = e };
                await leaderboardsSqlClient.SaveChangesAsync(replays, false, cancellationToken).ConfigureAwait(false);

                await leaderboardsSqlClient.SaveChangesAsync(entries, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}

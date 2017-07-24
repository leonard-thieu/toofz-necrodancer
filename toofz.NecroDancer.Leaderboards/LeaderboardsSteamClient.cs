using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards
{
    public class LeaderboardsSteamClient
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(LeaderboardsSteamClient));

        const int AppId = 247080;

        public LeaderboardsSteamClient()
        {
            User = Util.GetEnvVar("SteamUserName");
            Pass = Util.GetEnvVar("SteamPassword");

            networkListener = new ProgressDebugNetworkListener();
            steamClient = new SteamClient { DebugNetworkListener = networkListener };
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();
            steamUserStats = steamClient.GetHandler<SteamUserStats>();

            manager.Subscribe<SteamUser.LoggedOffCallback>(response =>
            {
                Log.Info("Logged off from Steam.");
            });
        }

        readonly string User;
        readonly string Pass;
        readonly ProgressDebugNetworkListener networkListener;
        readonly SteamClient steamClient;
        readonly CallbackManager manager;
        readonly SteamUser steamUser;
        readonly SteamUserStats steamUserStats;

        public IProgress<long> Progress
        {
            get { return networkListener.Progress; }
            set { networkListener.Progress = value; }
        }

        public Task<SteamClient.ConnectedCallback> ConnectAsync()
        {
            var tcs = new TaskCompletionSource<SteamClient.ConnectedCallback>();

            IDisposable onConnected = null;
            IDisposable onDisconnected = null;
            onConnected = manager.Subscribe<SteamClient.ConnectedCallback>(response =>
            {
                switch (response.Result)
                {
                    case EResult.OK:
                        Log.Info("Connected to Steam.");
                        tcs.TrySetResult(response);
                        break;
                    default:
                        tcs.TrySetException(new SteamKitException($"Unable to connect to Steam.") { Result = response.Result });
                        break;
                }

                onConnected.Dispose();
                onDisconnected.Dispose();
            });
            onDisconnected = manager.Subscribe<SteamClient.DisconnectedCallback>(response =>
            {
                tcs.TrySetException(new SteamKitException("Unable to connect to Steam."));
                onConnected.Dispose();
                onDisconnected.Dispose();
            });

            steamClient.Connect();
            manager.RunWaitAllCallbacks(TimeSpan.FromSeconds(1));

            return tcs.Task;
        }

        public Task<SteamUser.LoggedOnCallback> LogOnAsync()
        {
            var tcs = new TaskCompletionSource<SteamUser.LoggedOnCallback>();

            IDisposable onLoggedOn = null;
            IDisposable onDisconnected = null;
            onLoggedOn = manager.Subscribe<SteamUser.LoggedOnCallback>(response =>
            {
                switch (response.Result)
                {
                    case EResult.OK:
                        Log.Info("Logged on to Steam.");
                        tcs.TrySetResult(response);
                        break;
                    case EResult.AccountLogonDenied:
                        {
                            var ex = new SteamKitException("Unable to logon to Steam: This account is SteamGuard protected.")
                            {
                                Result = response.Result
                            };
                            tcs.TrySetException(ex);
                            break;
                        }
                    default:
                        {
                            var ex = new SteamKitException("Unable to logon to Steam.")
                            {
                                Result = response.Result
                            };
                            tcs.TrySetException(ex);
                            break;
                        }
                }

                onLoggedOn.Dispose();
                onDisconnected.Dispose();
            });
            onDisconnected = manager.Subscribe<SteamClient.DisconnectedCallback>(response =>
            {
                tcs.TrySetException(new SteamKitException("Unable to connect to Steam."));
                onLoggedOn.Dispose();
                onDisconnected.Dispose();
            });

            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = User,
                Password = Pass,
            });
            manager.RunWaitAllCallbacks(TimeSpan.FromSeconds(1));

            return tcs.Task;
        }

        static readonly SemaphoreSlim connectAndLogOnSemaphore = new SemaphoreSlim(1, 1);

        async Task ConnectAndLogOnAsync()
        {
            await connectAndLogOnSemaphore.WaitAsync();
            try
            {
                if (!steamClient.IsConnected)
                {
                    await ConnectAsync().ConfigureAwait(false);
                }
                if (steamClient.SessionID == null)
                {
                    await LogOnAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                connectAndLogOnSemaphore.Release();
            }
        }

        public async Task<Leaderboard> GetLeaderboardAsync(Header header)
        {
            await ConnectAndLogOnAsync().ConfigureAwait(false);

            var lbid = header.id;

            var leaderboard = new Leaderboard
            {
                LeaderboardId = lbid,
                CharacterId = Enumeration.Parse<Character>(header.character, true).Id,
                RunId = Enumeration.Parse<Run>(header.run.Replace(" ", ""), true).Id,
                LastUpdate = DateTime.UtcNow,
            };

            try
            {
                var response = await steamUserStats
                    .GetLeaderboardEntries(AppId, lbid, 0, 1000000, ELeaderboardDataRequest.Global)
                    .ToTask()
                    .ConfigureAwait(false);

                switch (response.Result)
                {
                    case EResult.OK:
                        {
                            leaderboard.EntriesCount = response.EntryCount;
                            var entries = response.Entries.Select(e =>
                            {
                                var entry = new Entry
                                {
                                    LeaderboardId = lbid,
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
                            leaderboard.Entries.AddRange(entries);

                            return leaderboard;
                        }
                    default:
                        throw new SteamKitException($"Unable to retrieve entries for leaderboard '{lbid}'.")
                        {
                            Result = response.Result
                        };
                }

            }
            catch (TaskCanceledException ex)
            {
                throw new SteamKitException($"Unable to retrieve entries for leaderboard '{lbid}'.", ex);
            }
        }

        public async Task<DailyLeaderboard> GetLeaderboardAsync(DailyHeader header)
        {
            await ConnectAndLogOnAsync().ConfigureAwait(false);

            var lbid = header.id;

            var leaderboard = new DailyLeaderboard
            {
                LeaderboardId = lbid,
                Date = header.date,
                IsProduction = header.production
            };

            switch (header.product)
            {
                case "Classic": leaderboard.ProductId = 0; break;
                case "Amplified": leaderboard.ProductId = 1; break;
                default:
                    throw new ArgumentException($"'{header.product}' is not a valid product.");
            }

            try
            {
                var response = await steamUserStats
                    .GetLeaderboardEntries(AppId, lbid, 0, 1000000, ELeaderboardDataRequest.Global)
                    .ToTask()
                    .ConfigureAwait(false);

                switch (response.Result)
                {
                    case EResult.OK:
                        {
                            leaderboard.LastUpdate = DateTime.UtcNow;

                            var entries = response.Entries.Select(e =>
                            {
                                var entry = new DailyEntry
                                {
                                    LeaderboardId = lbid,
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
                            leaderboard.Entries.AddRange(entries);

                            return leaderboard;
                        }
                    default:
                        throw new SteamKitException($"Unable to retrieve entries for leaderboard '{lbid}'.")
                        {
                            Result = response.Result
                        };
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new SteamKitException($"Unable to retrieve entries for leaderboard '{lbid}'.", ex);
            }
        }

        public async Task<DailyHeader> GetHeaderAsync(DateTime date, string product, bool isProduction)
        {
            await ConnectAndLogOnAsync().ConfigureAwait(false);

            var header = new DailyHeader
            {
                date = date,
                product = product,
                production = isProduction
            };

            var tokens = new List<string>();

            switch (product)
            {
                case "Amplified": tokens.Add("DLC"); break;
                case "Classic": break;
                default:
                    throw new ArgumentException($"'{product}' is not a valid product.");
            }

            tokens.Add(date.ToString("d/M/yyyy"));

            var name = string.Join(" ", tokens);
            if (isProduction) { name += "_PROD"; }

            try
            {
                var response = await steamUserStats
                    .FindLeaderboard(AppId, name)
                    .ToTask()
                    .ConfigureAwait(false);

                switch (response.Result)
                {
                    case EResult.OK: header.id = response.ID; break;
                    default:
                        throw new SteamKitException($"Unable to find the leaderboard '{name}'.")
                        {
                            Result = response.Result
                        };
                }

                return header;
            }
            catch (TaskCanceledException ex)
            {
                throw new SteamKitException($"Unable to retrieve data for the leaderboard '{name}'.", ex);
            }
        }

        class ProgressDebugNetworkListener : IDebugNetworkListener
        {
            public IProgress<long> Progress { get; set; }

            public void OnIncomingNetworkMessage(EMsg msgType, byte[] data)
            {
                Progress?.Report(data.Length);
            }

            public void OnOutgoingNetworkMessage(EMsg msgType, byte[] data)
            {
                // Do nothing
            }
        }
    }
}

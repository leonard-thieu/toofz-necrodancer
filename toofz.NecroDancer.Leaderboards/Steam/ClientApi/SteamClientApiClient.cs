﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public sealed class SteamClientApiClient
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientApiClient));

        const int AppId = 247080;
        const int EntriesPerRequest = 1000000;

        public SteamClientApiClient()
        {
            var leaderboardsService = new LeaderboardsService();
            categories = leaderboardsService.ReadCategories("leaderboard-categories.json");

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

        readonly Categories categories;
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

        Task<SteamClient.ConnectedCallback> ConnectAsync()
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
                        tcs.TrySetException(new SteamClientApiException($"Unable to connect to Steam.") { Result = response.Result });
                        break;
                }

                onConnected.Dispose();
                onDisconnected.Dispose();
            });
            onDisconnected = manager.Subscribe<SteamClient.DisconnectedCallback>(response =>
            {
                tcs.TrySetException(new SteamClientApiException("Unable to connect to Steam."));
                onConnected.Dispose();
                onDisconnected.Dispose();
            });

            steamClient.Connect();
            manager.RunWaitAllCallbacks(TimeSpan.FromSeconds(1));

            return tcs.Task;
        }

        Task<SteamUser.LoggedOnCallback> LogOnAsync()
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
                            var ex = new SteamClientApiException("Unable to logon to Steam: This account is SteamGuard protected.")
                            {
                                Result = response.Result
                            };
                            tcs.TrySetException(ex);
                            break;
                        }
                    default:
                        {
                            var ex = new SteamClientApiException("Unable to logon to Steam.")
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
                tcs.TrySetException(new SteamClientApiException("Unable to connect to Steam."));
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

        public async Task<Leaderboard> GetLeaderboardAsync(LeaderboardHeader header)
        {
            await ConnectAndLogOnAsync().ConfigureAwait(false);

            var lbid = header.id;

            var leaderboard = new Leaderboard
            {
                LeaderboardId = lbid,
                CharacterId = categories.GetItemId("characters", header.character),
                RunId = categories.GetItemId("runs", header.run),
                LastUpdate = DateTime.UtcNow,
            };

            try
            {
                var response = await steamUserStats
                    .GetLeaderboardEntries(AppId, lbid, 0, EntriesPerRequest, ELeaderboardDataRequest.Global)
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
                        throw new SteamClientApiException($"Unable to retrieve entries for leaderboard '{lbid}'.")
                        {
                            Result = response.Result
                        };
                }

            }
            catch (TaskCanceledException ex)
            {
                throw new SteamClientApiException($"Unable to retrieve entries for leaderboard '{lbid}'.", ex);
            }
        }

        public async Task<DailyLeaderboard> GetDailyLeaderboardAsync(DailyLeaderboardHeader header)
        {
            await ConnectAndLogOnAsync().ConfigureAwait(false);

            var lbid = header.id;

            var leaderboard = new DailyLeaderboard
            {
                LeaderboardId = lbid,
                Date = header.date,
                ProductId = categories.GetItemId("products", header.product),
                IsProduction = header.production,
            };

            try
            {
                var response = await steamUserStats
                    .GetLeaderboardEntries(AppId, lbid, 0, EntriesPerRequest, ELeaderboardDataRequest.Global)
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
                        throw new SteamClientApiException($"Unable to retrieve entries for leaderboard '{lbid}'.")
                        {
                            Result = response.Result
                        };
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new SteamClientApiException($"Unable to retrieve entries for leaderboard '{lbid}'.", ex);
            }
        }

        public async Task<DailyLeaderboardHeader> GetDailyLeaderboardHeaderAsync(DateTime date, string product, bool isProduction)
        {
            await ConnectAndLogOnAsync().ConfigureAwait(false);

            var header = new DailyLeaderboardHeader
            {
                date = date,
                product = product,
                production = isProduction,
            };

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
                        throw new SteamClientApiException($"Unable to find the leaderboard '{name}'.")
                        {
                            Result = response.Result
                        };
                }

                return header;
            }
            catch (TaskCanceledException ex)
            {
                throw new SteamClientApiException($"Unable to retrieve data for the leaderboard '{name}'.", ex);
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
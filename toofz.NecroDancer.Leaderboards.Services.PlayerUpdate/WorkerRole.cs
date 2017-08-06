using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using toofz.NecroDancer.Leaderboards.Services.Common;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;

namespace toofz.NecroDancer.Leaderboards.Services.PlayerUpdate
{
    sealed class WorkerRole : WorkerRoleBase<PlayerSettings>
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(WorkerRole));

        public WorkerRole() : base("toofz Player Service") { }

        OAuth2Handler oAuth2Handler;
        HttpMessageHandler apiHandlers;

        protected override string SettingsPath => "player-settings.json";

        protected override void OnStartOverride()
        {
            oAuth2Handler = new OAuth2Handler();
            apiHandlers = HttpClientFactory.CreatePipeline(new WebRequestHandler(), new DelegatingHandler[]
            {
                new LoggingHandler(),
                new EnsureSuccessHandler(),
                oAuth2Handler,
            });
        }

        protected override async Task RunAsyncOverride(CancellationToken cancellationToken)
        {
            oAuth2Handler.UserName = Util.GetEnvVar("PlayersUserName");
            oAuth2Handler.Password = Util.GetEnvVar("PlayersPassword");
            var apiBaseAddress = Util.GetEnvVar("toofzApiBaseAddress");
            var steamWebApiKey = Util.GetEnvVar("SteamWebApiKey");

            var steamApiHandlers = HttpClientFactory.CreatePipeline(new WebRequestHandler(), new DelegatingHandler[]
            {
                new LoggingHandler(),
                new SteamWebApiTransientFaultHandler(Application.TelemetryClient),
            });

            using (var apiClient = new ApiClient(apiHandlers))
            using (var steamWebApiClient = new SteamWebApiClient(steamApiHandlers))
            {
                apiClient.BaseAddress = new Uri(apiBaseAddress);

                steamWebApiClient.SteamWebApiKey = steamWebApiKey;

                await UpdatePlayersAsync(
                    apiClient,
                    steamWebApiClient,
                    Settings.PlayersPerUpdate,
                    cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        internal async Task UpdatePlayersAsync(
            IApiClient apiClient,
            ISteamWebApiClient steamWebApiClient,
            int limit,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (apiClient == null)
                throw new ArgumentNullException(nameof(apiClient), $"{nameof(apiClient)} is null.");
            if (steamWebApiClient == null)
                throw new ArgumentNullException(nameof(steamWebApiClient), $"{nameof(steamWebApiClient)} is null.");
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
    }
}

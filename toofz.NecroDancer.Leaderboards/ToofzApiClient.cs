using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class ToofzApiClient : HttpClient, IToofzApiClient
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(ToofzApiClient));

        /// <summary>
        /// Initializes a new instance of the <see cref="ToofzApiClient"/> class with a specific handler.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        public ToofzApiClient(HttpMessageHandler handler) : base(handler, disposeHandler: false) { }

        #region Players

        public async Task<IEnumerable<long>> GetStaleSteamIdsAsync(int limit, CancellationToken cancellationToken)
        {
            var response = await GetAsync($"players?limit={limit}", cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<long[]>(cancellationToken).ConfigureAwait(false);
        }

        public Task<string> PostPlayersAsync(IEnumerable<Player> players, CancellationToken cancellationToken)
        {
            var entities = (from p in players
                            select new
                            {
                                SteamId = p.SteamId,
                                Exists = p.Exists.Value,
                                Name = p.Name,
                                LastUpdate = p.LastUpdate.Value,
                                Avatar = p.Avatar,
                            }).ToList();

            return PostEntitiesAsync("players", entities, cancellationToken);
        }

        #endregion

        #region Replays

        public async Task<IEnumerable<long>> GetMissingReplayIdsAsync(int limit, CancellationToken cancellationToken)
        {
            var response = await GetAsync($"replays?limit={limit}", cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<IEnumerable<long>>(cancellationToken).ConfigureAwait(false);
        }

        public Task<string> PostReplaysAsync(IEnumerable<Replay> replays, CancellationToken cancellationToken)
        {
            return PostEntitiesAsync("replays", replays, cancellationToken);
        }

        #endregion

        async Task<string> PostEntitiesAsync<T>(string requestUri, IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            using (var activity = new StoreNotifier(Log, requestUri))
            {
                var response = await this.PostAsJsonAsync(requestUri, entities, cancellationToken).ConfigureAwait(false);
                if (response.Content.Headers.ContentType.MediaType != "application/json")
                {
                    throw await ApiException.CreateIncorrectMediaTypeAsync(response).ConfigureAwait(false);
                }

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                try
                {
                    var bulkStore = JObject.Parse(json);
                    var rowsAffectedObj = bulkStore["rowsAffected"];
                    var rowsAffected = rowsAffectedObj.Value<long>();
                    activity.Progress.Report(rowsAffected);
                }
                catch (JsonReaderException ex)
                {
                    var message = $"Couldn't deserialize response from {requestUri}." + Environment.NewLine + json;
                    Log.Error(message, ex);
                }

                return json;
            }
        }
    }
}
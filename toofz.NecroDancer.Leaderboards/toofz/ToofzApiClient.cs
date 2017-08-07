using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class ToofzApiClient : HttpClient, IToofzApiClient
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(ToofzApiClient));

        /// <summary>
        /// Initializes a new instance of the <see cref="ToofzApiClient"/> class with a specific handler.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        public ToofzApiClient(HttpMessageHandler handler) : base(handler, disposeHandler: false)
        {
            MaxResponseContentBufferSize = 2 * 1024 * 1024;
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        #region Players

        public async Task<Players> GetPlayersAsync(
            GetPlayersParams @params = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = "players"
                .SetQueryParams(new
                {
                    q = @params?.Query,
                    offset = @params?.Offset,
                    limit = @params?.Limit,
                    sort = @params.Sort,
                });

            var response = await GetAsync(url, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<Players>(cancellationToken).ConfigureAwait(false);
        }

        public Task<string> PostPlayersAsync(
            IEnumerable<Leaderboards.Player> players,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players), $"{nameof(players)} is null.");

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

        public async Task<Replays> GetReplaysAsync(
            GetReplaysParams @params = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = "replays"
                .SetQueryParams(new
                {
                    version = @params?.Version,
                    error = @params?.ErrorCode,
                    offset = @params?.Offset,
                    limit = @params?.Limit,
                });

            var response = await GetAsync(url, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<Replays>(cancellationToken).ConfigureAwait(false);
        }

        public Task<string> PostReplaysAsync(
            IEnumerable<Leaderboards.Replay> replays,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (replays == null)
                throw new ArgumentNullException(nameof(replays), $"{nameof(replays)} is null.");

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
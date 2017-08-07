using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using log4net;

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

        public async Task<BulkStore> PostPlayersAsync(
            IEnumerable<Leaderboards.Player> players,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players), $"{nameof(players)} is null.");

            var response = await this.PostAsJsonAsync("players", players, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<BulkStore>(cancellationToken).ConfigureAwait(false);
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

        public async Task<BulkStore> PostReplaysAsync(
            IEnumerable<Leaderboards.Replay> replays,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (replays == null)
                throw new ArgumentNullException(nameof(replays), $"{nameof(replays)} is null.");

            var response = await this.PostAsJsonAsync("replays", replays, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<BulkStore>(cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}
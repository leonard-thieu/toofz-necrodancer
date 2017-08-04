using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using log4net;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.SteamWebApi.ISteamRemoteStorage;
using toofz.NecroDancer.Leaderboards.SteamWebApi.ISteamUser;

namespace toofz.NecroDancer.Leaderboards.SteamWebApi
{
    public sealed class SteamWebApiClient : HttpClient, ISteamWebApiClient
    {
        #region Static Members

        static readonly ILog Log = LogManager.GetLogger(typeof(SteamWebApiClient));

        static readonly Uri SteamApiUri = new Uri("https://api.steampowered.com/");

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamWebApiClient"/> class with a specific handler.
        /// </summary>
        /// <param name="handler">
        /// The HTTP handler stack to use for sending requests.
        /// </param>
        public SteamWebApiClient(HttpMessageHandler handler) : base(handler)
        {
            MaxResponseContentBufferSize = 2 * 1024 * 1024;
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        #endregion

        #region Fields

        /// <summary>
        /// A Steam Web API key. This is required by some API endpoints.
        /// </summary>
        public string SteamWebApiKey { get; set; }

        #endregion

        #region GetPlayerSummaries

        static readonly UriTemplate GetPlayerSummariesUri = new UriTemplate("ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={steamIds}");

        /// <summary>
        /// The maximum number of Steam IDs allowed per request by <see cref="GetPlayerSummariesAsync"/>.
        /// </summary>
        public const int MaxPlayerSummariesPerRequest = 100;

        /// <summary>
        /// Returns basic profile information for a list of 64-bit Steam IDs.
        /// </summary>
        /// <param name="steamIds">
        /// List of 64 bit Steam IDs to return profile information for. Up to 100 Steam IDs can be requested.
        /// </param>
        /// <param name="progress">
        /// A progress provider that will be called with total bytes requested. <paramref name="progress"/> may be null.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="GetPlayerSummariesAsync"/> requires <see cref="SteamWebApiKey"/> to be set to a valid Steam Web API Key.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="steamIds"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Unable to request more than <see cref="MaxPlayerSummariesPerRequest"/> player summaries.
        /// </exception>
        public async Task<PlayerSummaries> GetPlayerSummariesAsync(
            IEnumerable<long> steamIds,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (SteamWebApiKey == null)
                throw new InvalidOperationException($"{nameof(GetPlayerSummariesAsync)} requires {nameof(SteamWebApiKey)} to be set to a valid Steam Web API Key.");
            if (steamIds == null)
                throw new ArgumentNullException(nameof(steamIds), $"{nameof(steamIds)} is null.");

            var ids = steamIds.ToList();
            if (ids.Count > MaxPlayerSummariesPerRequest)
                throw new ArgumentException($"Unable to request more than {MaxPlayerSummariesPerRequest} player summaries.", nameof(steamIds));

            var download = StreamPipeline.Create(this, progress, cancellationToken);

            PlayerSummaries playerSummaries = null;
            var processData = new ActionBlock<Stream>(data =>
            {
                using (var sr = new StreamReader(data))
                {
                    playerSummaries = JsonConvert.DeserializeObject<PlayerSummaries>(sr.ReadToEnd());
                }
            });

            download.LinkTo(processData, new DataflowLinkOptions { PropagateCompletion = true });

            var url = GetPlayerSummariesUri.BindByName(SteamApiUri, new Dictionary<string, string>
            {
                { "key", SteamWebApiKey },
                { "steamIds", string.Join(",", ids) },
            });
            await download.CheckSendAsync(url).ConfigureAwait(false);

            download.Complete();
            await processData.Completion.ConfigureAwait(false);

            Debug.Assert(playerSummaries != null);

            return playerSummaries;
        }

        #endregion

        #region GetUGCFileDetails

        static readonly UriTemplate GetUgcFileDetailsUri = new UriTemplate("ISteamRemoteStorage/GetUGCFileDetails/v1/?key={key}&appid={appId}&ugcid={ugcId}");

        // TODO: Handle a potential error response from the Steam Web API. See: https://wiki.teamfortress.com/wiki/WebAPI/GetUGCFileDetails#Result_data.
        /// <summary>
        /// Returns file details for a UGC ID.
        /// </summary>
        /// <param name="appId">
        /// The ID of the product of the UGC.
        /// </param>
        /// <param name="ugcId">
        /// The ID of the UGC to get file details for.
        /// </param>
        /// <param name="progress">
        /// A progress provider that will be called with total bytes requested. <paramref name="progress"/> may be null.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="GetUgcFileDetailsAsync"/> requires <see cref="SteamWebApiKey"/> to be set to a valid Steam Web API Key.
        /// </exception>
        public async Task<UgcFileDetails> GetUgcFileDetailsAsync(
            int appId,
            long ugcId,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (SteamWebApiKey == null)
                throw new InvalidOperationException($"{nameof(GetUgcFileDetailsAsync)} requires {nameof(SteamWebApiKey)} to be set to a valid Steam Web API Key.");

            var download = StreamPipeline.Create(this, progress, cancellationToken);

            UgcFileDetails ugcFileDetails = null;
            var processData = new ActionBlock<Stream>(data =>
            {
                using (var sr = new StreamReader(data))
                {
                    ugcFileDetails = JsonConvert.DeserializeObject<UgcFileDetails>(sr.ReadToEnd());
                }
            });

            download.LinkTo(processData, new DataflowLinkOptions { PropagateCompletion = true });

            var url = GetUgcFileDetailsUri.BindByName(SteamApiUri, new Dictionary<string, string>
            {
                { "key", SteamWebApiKey },
                { "appId", appId.ToString() },
                { "ugcId", ugcId.ToString() },
            });
            await download.CheckSendAsync(url).ConfigureAwait(false);

            download.Complete();
            await processData.Completion.ConfigureAwait(false);

            Debug.Assert(ugcFileDetails != null);

            return ugcFileDetails;
        }

        #endregion
    }
}
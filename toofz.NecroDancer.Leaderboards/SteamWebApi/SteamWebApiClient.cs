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
using toofz.NecroDancer.Leaderboards.SteamWebApi.ISteamUser;

namespace toofz.NecroDancer.Leaderboards.SteamWebApi
{
    public sealed class SteamWebApiClient : HttpClient
    {
        #region Static Members

        static readonly ILog Log = LogManager.GetLogger(typeof(SteamWebApiClient));

        const int AppId = 247080;
        static readonly Uri SteamApiUri = new Uri("https://api.steampowered.com/");

        static async Task SendAsync<T>(ITargetBlock<T> block, T item)
        {
            if (!await block.SendAsync(item).ConfigureAwait(false))
            {
                throw new InvalidOperationException($"Posting {item} to the get data block failed.");
            }
        }

        #endregion

        #region Initialization

        public SteamWebApiClient(HttpMessageHandler handler, LeaderboardsReader reader) : base(handler)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));

            MaxResponseContentBufferSize = 2 * 1024 * 1024;
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        #endregion

        #region Fields

        readonly LeaderboardsReader reader;

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
            await SendAsync(download, url).ConfigureAwait(false);

            download.Complete();
            await processData.Completion.ConfigureAwait(false);

            Debug.Assert(playerSummaries != null);

            return playerSummaries;
        }

        #endregion

        #region Replays

        static readonly UriTemplate GetUgcFileDetailsUri = new UriTemplate($"ISteamRemoteStorage/GetUGCFileDetails/v1/?format=xml&key={{key}}&appid={AppId}&ugcid={{ugcId}}");

        public async Task<IEnumerable<ReplayContext>> GetReplaysAsync(IEnumerable<long> ugcIds,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var activity = new DownloadNotifier(Log, "replays"))
            {
                var pipeline = ReplayPipeline.Create(this, activity.Progress, cancellationToken);

                var replayContexts = new List<ReplayContext>();
                var processData = new ActionBlock<ReplayContext>(context =>
                {
                    replayContexts.Add(context);
                });

                pipeline.LinkTo(processData, new DataflowLinkOptions { PropagateCompletion = true });

                foreach (var ugcId in ugcIds)
                {
                    var requestUri = GetUgcFileDetailsUri.BindByName(SteamApiUri, new Dictionary<string, string>
                    {
                        { "key", SteamWebApiKey },
                        { "ugcId", ugcId.ToString() },
                    });
                    await SendAsync(pipeline, new Tuple<long, Uri>(ugcId, requestUri)).ConfigureAwait(false);
                }

                pipeline.Complete();

                await processData.Completion.ConfigureAwait(false);

                return replayContexts;
            }
        }

        #endregion
    }
}
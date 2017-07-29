using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsHttpClient : HttpClient, ILeaderboardsHttpClient
    {
        #region Static Members

        const int AppId = 247080;
        static readonly Uri SteamApiUri = new Uri("http://api.steampowered.com/");

        const int PlayersPerRequest = 100;

        static readonly ILog Log = LogManager.GetLogger(typeof(LeaderboardsHttpClient));

        static async Task SendAsync<T>(ITargetBlock<T> block, T item)
        {
            if (!await block.SendAsync(item).ConfigureAwait(false))
            {
                throw new InvalidOperationException($"Posting {item} to the get data block failed.");
            }
        }

        #endregion

        #region Initialization

        public LeaderboardsHttpClient(HttpMessageHandler handler, ILeaderboardsReader reader) : base(handler)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.reader = reader;

            MaxResponseContentBufferSize = 2 * 1024 * 1024;
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        #endregion

        #region Fields

        readonly ILeaderboardsReader reader;

        public string SteamWebApiKey { get; set; }

        #endregion

        #region Players

        static readonly UriTemplate GetPlayerSummariesUri = new UriTemplate("ISteamUser/GetPlayerSummaries/v0002/?format=json&key={key}&steamids={steamIds}");

        public async Task<IEnumerable<Player>> GetPlayersAsync(IEnumerable<long> steamIds,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var activity = new DownloadNotifier(Log, "players"))
            {
                var count = steamIds.Count();
                var players = new List<Player>(count);

                var download = StreamPipeline.Create(this, activity.Progress, cancellationToken);

                var processData = new ActionBlock<Stream>(data =>
                {
                    players.AddRange(reader.ReadPlayers(data));
                });

                download.LinkTo(processData, new DataflowLinkOptions { PropagateCompletion = true });

                for (int i = 0; i < count; i += PlayersPerRequest)
                {
                    var ids = steamIds.Skip(i).Take(PlayersPerRequest);
                    var url = GetPlayerSummariesUri.BindByName(SteamApiUri, new Dictionary<string, string>
                    {
                        { "key", SteamWebApiKey },
                        { "steamIds", string.Join(",", ids) },
                    });
                    await SendAsync(download, url).ConfigureAwait(false);
                }

                download.Complete();
                await processData.Completion.ConfigureAwait(false);

                return players;
            }
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
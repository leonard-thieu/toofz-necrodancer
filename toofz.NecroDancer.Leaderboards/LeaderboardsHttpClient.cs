using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsHttpClient : HttpClient, ILeaderboardsHttpClient
    {
        #region Static Members

        private const int AppId = 247080;
        private static readonly Uri LeaderboardListUri = new Uri($"http://steamcommunity.com/stats/{AppId}/leaderboards/?xml=1");
        private static readonly Uri SteamCommunityUri = new Uri("http://steamcommunity.com/");
        private static readonly Uri SteamApiUri = new Uri("http://api.steampowered.com/");

        private const int EntriesPerPage = 5001;
        private const int PlayersPerRequest = 100;

        private static readonly ILog Log = LogManager.GetLogger(typeof(LeaderboardsHttpClient));

        private static async Task SendAsync<T>(ITargetBlock<T> block, T item)
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

        private readonly ILeaderboardsReader reader;

        public string SteamWebApiKey { get; set; }

        #endregion

        #region Leaderboard Headers

        public async Task<IEnumerable<LeaderboardHeader>> GetLeaderboardHeadersAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var activity = new DownloadNotifier(Log, "leaderboard headers"))
            {
                IEnumerable<LeaderboardHeader> leaderboardList = null;

                var download = StreamPipeline.Create(this, activity.Progress, cancellationToken);

                var processData = new ActionBlock<Stream>(data =>
                {
                    leaderboardList = reader.ReadLeaderboardHeaders(data);
                });

                download.LinkTo(processData, new DataflowLinkOptions { PropagateCompletion = true });

                await SendAsync(download, LeaderboardListUri).ConfigureAwait(false);
                download.Complete();
                await processData.Completion.ConfigureAwait(false);

                return leaderboardList;
            }
        }

        #endregion

        #region Leaderboards and Entries

        private static readonly UriTemplate LeaderboardsUriTemplate = new UriTemplate($"stats/{AppId}/leaderboards/{{leaderboardId}}/?xml=1&start={{start}}");

        public async Task<Leaderboard> GetLeaderboardAsync(LeaderboardHeader header,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Leaderboard leaderboard = null;
            var download = StreamPipeline.Create(this, progress, cancellationToken);

            var expectedCount = 1;
            var processedCount = 0;
            var processData = new ActionBlock<Stream>(async data =>
            {
                var lb = reader.ReadLeaderboard(data);

                if (leaderboard == null)
                {
                    // Set last update time and merge header
                    lb.LastUpdate = DateTime.UtcNow;
                    lb.CharacterId = header.Character.Id;
                    lb.RunId = header.Run.Id;
                    lb.Date = header.Date;

                    // Add remaining pages (if any) to the pipeline
                    var remainingPageCount = lb.EntriesCount / EntriesPerPage;
                    if (remainingPageCount > 0)
                    {
                        Interlocked.Add(ref expectedCount, remainingPageCount);
                        var leaderboardId = lb.LeaderboardId.ToString();
                        for (int i = 0; i < remainingPageCount; i++)
                        {
                            var start = (i + 1) * EntriesPerPage;
                            var address = LeaderboardsUriTemplate.BindByName(SteamCommunityUri, new Dictionary<string, string>
                            {
                                { "leaderboardId", leaderboardId },
                                { "start", start.ToString() },
                            });
                            await SendAsync(download, address).ConfigureAwait(false);
                        }
                    }

                    leaderboard = lb;
                }
                else
                {
                    // Received one of the remaining pages, so just add the entries to the existing leaderboard
                    lock (((ICollection)leaderboard.Entries).SyncRoot)
                    {
                        leaderboard.Entries.AddRange(lb.Entries);
                    }
                }

                Interlocked.Increment(ref processedCount);
                if (processedCount == expectedCount)
                {
                    download.Complete();
                }
                Debug.Assert(processedCount <= expectedCount);
            });

            download.LinkTo(processData, new DataflowLinkOptions { PropagateCompletion = true });

            await SendAsync(download, new Uri(header.Address)).ConfigureAwait(false);

            await processData.Completion.ConfigureAwait(false);

            return leaderboard;
        }

        #endregion

        #region Players

        private static readonly UriTemplate GetPlayerSummariesUri = new UriTemplate("ISteamUser/GetPlayerSummaries/v0002/?format=json&key={key}&steamids={steamIds}");

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

        private static readonly UriTemplate GetUgcFileDetailsUri = new UriTemplate($"ISteamRemoteStorage/GetUGCFileDetails/v1/?format=xml&key={{key}}&appid={AppId}&ugcid={{ugcId}}");

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
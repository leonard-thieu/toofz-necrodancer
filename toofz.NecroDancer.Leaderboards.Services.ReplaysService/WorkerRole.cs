using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using toofz.NecroDancer.Leaderboards.Services.Common;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using toofz.NecroDancer.Replays;

namespace toofz.NecroDancer.Leaderboards.Services.ReplaysService
{
    sealed class WorkerRole : WorkerRoleBase<ReplaySettings>
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(WorkerRole));

        const uint AppId = 247080;

        static CloudBlobDirectory GetDirectory(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("crypt");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            return container.GetDirectoryReference("replays");
        }

        public WorkerRole() : base("toofz Replays Service") { }

        OAuth2Handler oAuth2Handler;
        HttpMessageHandler apiHandlers;

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
            oAuth2Handler.UserName = Util.GetEnvVar("ReplaysUserName");
            oAuth2Handler.Password = Util.GetEnvVar("ReplaysPassword");
            var apiBaseAddress = Util.GetEnvVar("toofzApiBaseAddress");
            var steamWebApiKey = Util.GetEnvVar("SteamWebApiKey");
            var storageConnectionString = Util.GetEnvVar("toofzStorageConnectionString");

            var steamApiHandlers = HttpClientFactory.CreatePipeline(new WebRequestHandler(), new DelegatingHandler[]
            {
                new LoggingHandler(),
                new SteamWebApiTransientFaultHandler(Application.TelemetryClient),
            });

            var ugcHandlers = HttpClientFactory.CreatePipeline(new WebRequestHandler(), new DelegatingHandler[]
            {
                new LoggingHandler(),
                new HttpRequestStatusHandler(),
            });

            using (var toofzApiClient = new ToofzApiClient(apiHandlers))
            using (var steamWebApiClient = new SteamWebApiClient(steamApiHandlers))
            using (var ugcHttpClient = new UgcHttpClient(ugcHandlers))
            {
                toofzApiClient.BaseAddress = new Uri(apiBaseAddress);

                steamWebApiClient.SteamWebApiKey = steamWebApiKey;

                await UpdateReplaysAsync(
                    toofzApiClient,
                    steamWebApiClient,
                    ugcHttpClient,
                    GetDirectory(storageConnectionString),
                    Settings.ReplaysPerUpdate,
                    cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        static readonly ReplaySerializer ReplaySerializer = new ReplaySerializer();

        internal async Task UpdateReplaysAsync(
            IToofzApiClient toofzApiClient,
            ISteamWebApiClient steamWebApiClient,
            IUgcHttpClient ugcHttpClient,
            CloudBlobDirectory directory,
            int limit,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (toofzApiClient == null)
                throw new ArgumentNullException(nameof(toofzApiClient), $"{nameof(toofzApiClient)} is null.");
            if (steamWebApiClient == null)
                throw new ArgumentNullException(nameof(steamWebApiClient), $"{nameof(steamWebApiClient)} is null.");
            if (ugcHttpClient == null)
                throw new ArgumentNullException(nameof(ugcHttpClient), $"{nameof(ugcHttpClient)} is null.");
            if (directory == null)
                throw new ArgumentNullException(nameof(directory), $"{nameof(directory)} is null.");
            if (limit <= 0)
                throw new ArgumentOutOfRangeException(nameof(limit));

            using (new UpdateNotifier(Log, "replays"))
            {
                var missing = await toofzApiClient.GetMissingReplayIdsAsync(limit, cancellationToken).ConfigureAwait(false);

                var replays = new ConcurrentBag<Replay>();
                using (var download = new DownloadNotifier(Log, "replays"))
                {
                    var requests = new List<Task>();
                    foreach (var ugcId in missing)
                    {
                        var request = UpdateReplayAsync(ugcId);
                        requests.Add(request);
                    }
                    await Task.WhenAll(requests).ConfigureAwait(false);

                    async Task UpdateReplayAsync(long ugcId)
                    {
                        var replay = new Replay { ReplayId = ugcId };
                        replays.Add(replay);

                        try
                        {
                            var ugcFileDetails = await steamWebApiClient.GetUgcFileDetailsAsync(AppId, ugcId, download.Progress, cancellationToken).ConfigureAwait(false);
                            try
                            {
                                var ugcFile = await ugcHttpClient.GetUgcFileAsync(ugcFileDetails.Data.Url, download.Progress, cancellationToken).ConfigureAwait(false);
                                try
                                {
                                    var replayData = ReplaySerializer.Deserialize(ugcFile);
                                    replay.Version = replayData.Header.Version;
                                    replay.KilledBy = replayData.Header.KilledBy;
                                    if (replayData.TryGetSeed(out int seed))
                                    {
                                        replay.Seed = seed;
                                    }

                                    ugcFile.Dispose();
                                    ugcFile = new MemoryStream();
                                    ReplaySerializer.Serialize(ugcFile, replayData);
                                    ugcFile.Position = 0;
                                }
                                // TODO: Catch a more specific exception.
                                catch (Exception ex)
                                {
                                    Log.Error($"Unable to read replay from '{ugcFileDetails.Data.Url}'.", ex);
                                    // Upload unmodified data on failure
                                    ugcFile.Position = 0;
                                }
                                finally
                                {
                                    try
                                    {
                                        var blob = directory.GetBlockBlobReference(replay.FileName);
                                        blob.Properties.ContentType = "application/octet-stream";
                                        blob.Properties.CacheControl = "max-age=604800"; // 1 week

                                        await blob.UploadFromStreamAsync(ugcFile, cancellationToken).ConfigureAwait(false);

                                        Log.Debug(blob.Uri);
                                    }
                                    // TODO: Catch a more specific exception.
                                    catch (Exception ex)
                                    {
                                        Log.Error($"Failed to upload {replay.FileName}.", ex);
                                    }
                                }
                            }
                            catch (HttpRequestStatusException ex)
                            {
                                replay.ErrorCode = -(int)ex.StatusCode;
                            }
                        }
                        catch (HttpRequestStatusException ex)
                        {
                            replay.ErrorCode = (int)ex.StatusCode;
                        }
                    }
                }

                // TODO: Add rollback to stored replays in case this fails
                await toofzApiClient.PostReplaysAsync(replays, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}

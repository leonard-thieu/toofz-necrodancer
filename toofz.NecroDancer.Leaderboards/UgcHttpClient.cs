using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class UgcHttpClient : HttpClient, IUgcHttpClient
    {
        #region Initialization

        public UgcHttpClient(HttpMessageHandler handler) : base(handler)
        {
            MaxResponseContentBufferSize = 2 * 1024 * 1024;
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        #endregion

        #region GetUgcFile

        public async Task<Stream> GetUgcFileAsync(
            string url,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url), $"{nameof(url)} is null.");

            var download = StreamPipeline.Create(this, progress, cancellationToken);

            Stream replayData = null;
            var processData = new ActionBlock<Stream>(data =>
            {
                replayData = data;
            });

            download.LinkTo(processData, new DataflowLinkOptions { PropagateCompletion = true });

            await download.CheckSendAsync(new Uri(url)).ConfigureAwait(false);

            download.Complete();
            await processData.Completion.ConfigureAwait(false);

            Debug.Assert(replayData != null);

            return replayData;
        }

        #endregion
    }
}

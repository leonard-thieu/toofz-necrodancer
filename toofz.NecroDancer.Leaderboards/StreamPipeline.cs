using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace toofz.NecroDancer.Leaderboards
{
    static class StreamPipeline
    {
        public static IPropagatorBlock<Uri, Stream> Create(
            HttpClient httpClient,
            IProgress<long> progress,
            CancellationToken cancellationToken)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            TransformBlock<Uri, HttpResponseMessage> request = CreateRequestBlock(httpClient, cancellationToken);
            TransformBlock<HttpResponseMessage, HttpContent> download = CreateDownloadBlock();
            TransformBlock<HttpContent, Stream> processContent = CreateProcessContentBlock(progress);

            request.LinkTo(download, new DataflowLinkOptions { PropagateCompletion = true });
            download.LinkTo(processContent, new DataflowLinkOptions { PropagateCompletion = true });

            return DataflowBlock.Encapsulate(request, processContent);
        }

        internal static TransformBlock<Uri, HttpResponseMessage> CreateRequestBlock(
            HttpClient httpClient,
            CancellationToken cancellationToken)
        {
            return new TransformBlock<Uri, HttpResponseMessage>(
                requestUri => httpClient.GetAsync(requestUri, cancellationToken),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded });
        }

        internal static TransformBlock<HttpResponseMessage, HttpContent> CreateDownloadBlock()
        {
            return new TransformBlock<HttpResponseMessage, HttpContent>(
                response => response.Content,
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded });
        }

        internal static TransformBlock<HttpContent, Stream> CreateProcessContentBlock(IProgress<long> progress)
        {
            return new TransformBlock<HttpContent, Stream>(
                content => content.ProcessContentAsync(progress),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });
        }
    }
}

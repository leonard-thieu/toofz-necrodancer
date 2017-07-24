using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using System.Xml;
using System.Xml.Linq;

namespace toofz.NecroDancer.Leaderboards
{
    internal static class StreamPipeline
    {
        public static IPropagatorBlock<Uri, Stream> Create(HttpClient httpClient, IProgress<long> progress,
            CancellationToken cancellationToken)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            TransformBlock<Uri, HttpResponseMessage> request = CreateRequestBlock(httpClient, cancellationToken);
            TransformBlock<HttpResponseMessage, HttpContent> download = CreateDownloadBlock();
            TransformBlock<HttpContent, Stream> processContent = CreateProcessContentBlock(progress);
            TransformBlock<Stream, Stream> validateSteamResponse = CreateValidateSteamResponseBlock(progress);

            request.LinkTo(download, new DataflowLinkOptions { PropagateCompletion = true });
            download.LinkTo(processContent, new DataflowLinkOptions { PropagateCompletion = true });
            processContent.LinkTo(validateSteamResponse, new DataflowLinkOptions { PropagateCompletion = true });

            return DataflowBlock.Encapsulate(request, validateSteamResponse);
        }

        internal static TransformBlock<Uri, HttpResponseMessage> CreateRequestBlock(HttpClient httpClient, CancellationToken cancellationToken)
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
                content => DataflowHelper.ProcessContentAsync(content, progress),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });
        }

        internal static TransformBlock<Stream, Stream> CreateValidateSteamResponseBlock(IProgress<long> progress)
        {
            return new TransformBlock<Stream, Stream>(
                data =>
                {
                    try
                    {
                        var doc = XDocument.Load(data);
                        var error = doc.Root.Elements("error").FirstOrDefault();
                        if (error != null)
                        {
                            throw new HttpRequestException(error.Value);
                        }
                        else
                        {
                            data.Position = 0;

                            return data;
                        }
                    }
                    catch (XmlException)
                    {
                        data.Position = 0;

                        return data;
                    }
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace toofz.NecroDancer.Leaderboards
{
    internal static class ReplayPipeline
    {
        private static readonly LeaderboardsReader LeaderboardsReader = new LeaderboardsReader();

        public static IPropagatorBlock<Tuple<long, Uri>, ReplayContext> Create(HttpClient httpClient, IProgress<long> progress,
            CancellationToken cancellationToken)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            TransformBlock<Tuple<long, Uri>, ReplayContext> downloadDetails = CreateDownloadDetails(httpClient, cancellationToken);
            TransformBlock<ReplayContext, ReplayContext> processDetails = ProcessDetails(progress);
            TransformBlock<ReplayContext, ReplayContext> downloadData = CreateDownloadData(httpClient, cancellationToken);
            TransformBlock<ReplayContext, ReplayContext> processData = CreateProcessData(progress);

            downloadDetails.LinkTo(processDetails, new DataflowLinkOptions { PropagateCompletion = true });
            processDetails.LinkTo(downloadData, new DataflowLinkOptions { PropagateCompletion = true });
            downloadData.LinkTo(processData, new DataflowLinkOptions { PropagateCompletion = true });

            return DataflowBlock.Encapsulate(downloadDetails, processData);
        }

        internal static TransformBlock<Tuple<long, Uri>, ReplayContext> CreateDownloadDetails(HttpClient httpClient, CancellationToken cancellationToken)
        {
            return new TransformBlock<Tuple<long, Uri>, ReplayContext>(
                async ugcId =>
                {
                    var context = new ReplayContext { UgcId = ugcId.Item1 };

                    // Download details
                    // Set DetailsContent
                    var response = await httpClient.GetAsync(ugcId.Item2, cancellationToken).ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            // Couldn't find UGC file details
                            context.ErrorCode = (int?)HttpStatusCode.NotFound;
                        }
                        else
                        {
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    else
                    {
                        context.DetailsContent = response.Content;
                    }

                    return context;
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded });
        }

        internal static TransformBlock<ReplayContext, ReplayContext> ProcessDetails(IProgress<long> progress)
        {
            return new TransformBlock<ReplayContext, ReplayContext>(
                async context =>
                {
                    // DetailsContent will be null if GetUGCFileDetails returned a 404
                    if (context.DetailsContent != null)
                    {
                        // Process details
                        // Set DataUri
                        var details = await DataflowHelper.ProcessContentAsync(context.DetailsContent, progress).ConfigureAwait(false);
                        context.DataUri = LeaderboardsReader.ReadReplayUri(details);
                    }

                    return context;
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });
        }

        internal static TransformBlock<ReplayContext, ReplayContext> CreateDownloadData(HttpClient httpClient, CancellationToken cancellationToken)
        {
            return new TransformBlock<ReplayContext, ReplayContext>(
                async context =>
                {
                    // DataUri will be null if GetUGCFileDetails returned a 404
                    if (context.DataUri != null)
                    {
                        // Download data
                        // Set DataContent
                        var response = await httpClient.GetAsync(context.DataUri, cancellationToken).ConfigureAwait(false);

                        if (!response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                // Couldn't find UGC file
                                context.ErrorCode = -(int?)HttpStatusCode.NotFound;
                            }
                            else
                            {
                                response.EnsureSuccessStatusCode();
                            }
                        }
                        else
                        {
                            context.DataContent = response.Content;
                        }
                    }

                    return context;
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded });
        }

        internal static TransformBlock<ReplayContext, ReplayContext> CreateProcessData(IProgress<long> progress)
        {
            return new TransformBlock<ReplayContext, ReplayContext>(
                 async context =>
                 {
                     // DataContent will be null if GetUGCFileDetails returned a 404
                     if (context.DataContent != null)
                     {
                         // Process data
                         // Set Data
                         var data = await DataflowHelper.ProcessContentAsync(context.DataContent, progress).ConfigureAwait(false);
                         context.Data = data;
                     }

                     return context;
                 },
                 new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });
        }
    }
}

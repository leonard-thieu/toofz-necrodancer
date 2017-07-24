using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class SteamTransientFaultHandler : DelegatingHandler
    {
        private static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        private static readonly RetryPolicy<SteamTransientErrorDetectionStrategy> RetryPolicy = SteamTransientErrorDetectionStrategy.Create(RetryStrategy);

        public SteamTransientFaultHandler(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        private readonly TelemetryClient telemetryClient;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                var startTime = DateTimeOffset.UtcNow;

                // Simple clone should handle use cases
                var clonedRequest = new HttpRequestMessage(HttpMethod.Get, request.RequestUri);
                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    if (telemetryClient != null)
                    {
                        var telemetry = new DependencyTelemetry
                        {
                            Name = request.RequestUri.Host,
                            CommandName = request.RequestUri.PathAndQuery,
                            StartTime = startTime,
                            Duration = DateTimeOffset.UtcNow - startTime,
                            Success = false,
                            DependencyKind = "HTTP",
                        };
                        telemetryClient.TrackDependency(telemetry);
                    }

                    throw new TransientHttpRequestException(response.StatusCode);
                }

                return response;
            }, cancellationToken);
        }
    }
}

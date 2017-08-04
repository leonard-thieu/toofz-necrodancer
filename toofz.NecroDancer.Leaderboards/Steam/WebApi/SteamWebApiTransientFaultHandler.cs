﻿using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi
{
    public sealed class SteamWebApiTransientFaultHandler : DelegatingHandler
    {
        static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        static readonly RetryPolicy<SteamWebApiTransientErrorDetectionStrategy> RetryPolicy = SteamWebApiTransientErrorDetectionStrategy.Create(RetryStrategy);

        public SteamWebApiTransientFaultHandler(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        readonly TelemetryClient telemetryClient;

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
                            Data = request.RequestUri.PathAndQuery,
                            Timestamp = startTime,
                            Duration = DateTimeOffset.UtcNow - startTime,
                            Success = false,
                            Type = "HTTP",
                        };
                        telemetryClient.TrackDependency(telemetry);
                    }

                    throw new HttpRequestStatusException { StatusCode = response.StatusCode };
                }

                return response;
            }, cancellationToken);
        }
    }
}

using System;
using System.IO;
using System.Net;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi
{
    public sealed class SteamWebApiTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        #region Static Members

        static readonly ILog Log = LogManager.GetLogger(typeof(SteamWebApiTransientErrorDetectionStrategy));

        public static RetryPolicy<SteamWebApiTransientErrorDetectionStrategy> Create(RetryStrategy retryStrategy)
        {
            var retryPolicy = new RetryPolicy<SteamWebApiTransientErrorDetectionStrategy>(retryStrategy);
            retryPolicy.Retrying += OnRetrying;

            return retryPolicy;
        }

        static void OnRetrying(object sender, RetryingEventArgs e)
        {
            Log.Debug(e.LastException.Message + $" Experienced a transient error during an HTTP request. Retrying ({e.CurrentRetryCount}) in {e.Delay}...");
        }

        #endregion

        #region ITransientErrorDetectionStrategy Members

        public bool IsTransient(Exception ex)
        {
            var transient = ex as HttpRequestStatusException;
            if (transient != null)
            {
                switch (transient.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.RequestTimeout:
                    case HttpStatusCode.InternalServerError:
                    case HttpStatusCode.BadGateway:
                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.GatewayTimeout:
                        return true;

                    default:
                        return false;
                }
            }

            if (ex is IOException)
                return true;

            return false;
        }

        #endregion
    }
}
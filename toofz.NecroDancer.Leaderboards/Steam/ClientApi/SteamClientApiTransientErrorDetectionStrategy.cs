using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    sealed class SteamClientApiTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        #region Static Members

        static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientApiTransientErrorDetectionStrategy));

        public static RetryPolicy<SteamClientApiTransientErrorDetectionStrategy> Create(RetryStrategy retryStrategy)
        {
            var retryPolicy = new RetryPolicy<SteamClientApiTransientErrorDetectionStrategy>(retryStrategy);
            retryPolicy.Retrying += OnRetrying;

            return retryPolicy;
        }

        static void OnRetrying(object sender, RetryingEventArgs e)
        {
            Log.Debug(e.LastException.Message + $" Experienced a transient error during a Steam Client API request. Retrying ({e.CurrentRetryCount}) in {e.Delay}...");
        }

        #endregion

        #region ITransientErrorDetectionStrategy Members

        /// <summary>
        /// Determines whether the specified exception represents a transient failure that
        /// can be compensated by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>
        /// true if the specified exception is considered as transient; otherwise, false.
        /// </returns>
        public bool IsTransient(Exception ex)
        {
            var transient = ex as SteamClientApiException;
            if (transient != null)
            {
                return transient.InnerException is TaskCanceledException;
            }
            return false;
        }

        #endregion
    }
}

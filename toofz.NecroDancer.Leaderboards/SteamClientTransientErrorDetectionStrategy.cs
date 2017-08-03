using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards
{
    class SteamClientTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        #region Static Members

        static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientTransientErrorDetectionStrategy));

        public static RetryPolicy<SteamClientTransientErrorDetectionStrategy> Create(RetryStrategy retryStrategy)
        {
            var retryPolicy = new RetryPolicy<SteamClientTransientErrorDetectionStrategy>(retryStrategy);
            retryPolicy.Retrying += OnRetrying;

            return retryPolicy;
        }

        static void OnRetrying(object sender, RetryingEventArgs e)
        {
            Log.Debug(e.LastException.Message + $" Experienced a transient error during a Steam Client API request. Retrying ({e.CurrentRetryCount}) in {e.Delay}...");
        }

        #endregion

        #region ITransientErrorDetectionStrategy Members

        public bool IsTransient(Exception ex)
        {
            var transient = ex as SteamKitException;
            if (transient != null)
            {
                return transient.InnerException is TaskCanceledException;
            }
            return false;
        }

        #endregion
    }
}

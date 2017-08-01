using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace toofz.NecroDancer.Leaderboards.Services.Common
{
    sealed class Idle
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Idle));

        readonly DateTime startTime = DateTime.UtcNow;

        public void Delay(TimeSpan updateInterval, CancellationToken cancellationToken)
        {
            DelayAsync(updateInterval, cancellationToken).Wait();
        }

        async Task DelayAsync(TimeSpan updateInterval, CancellationToken cancellationToken)
        {
            var remaining = GetRemainingTime(updateInterval);
            LogTimeRemaining(remaining);
            if (remaining > Constants.MaxServicePointIdleTime)
            {
                await Task.Delay(Constants.MaxServicePointIdleTime, cancellationToken).ConfigureAwait(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            remaining = GetRemainingTime(updateInterval);
            if (remaining > TimeSpan.Zero)
            {
                await Task.Delay(remaining, cancellationToken).ConfigureAwait(false);
            }
        }

        static void LogTimeRemaining(TimeSpan remaining)
        {
            if (remaining > TimeSpan.Zero)
            {
                Log.Info($"Next run takes place in {remaining.TotalSeconds:F0} seconds...");
            }
            else
            {
                Log.Info("Next run starting immediately...");
            }
        }

        TimeSpan GetRemainingTime(TimeSpan updateInterval) => updateInterval - (DateTime.UtcNow - startTime);
    }
}

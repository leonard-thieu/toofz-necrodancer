using System.Diagnostics;
using System.Globalization;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class UpdateNotifier : NotifierBase
    {
        public UpdateNotifier(ILog log, string name) : base("Update", log, name) { }

        #region IDisposable Members

        bool disposed;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                var duration = Stopwatch.Elapsed.TotalSeconds.ToString("F1", CultureInfo.CurrentCulture);
                Log.Info($"{Category} {Name} complete after {duration} s.");
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}

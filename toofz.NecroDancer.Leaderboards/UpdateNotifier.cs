using System.Diagnostics;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class UpdateNotifier : NotifierBase
    {
        public UpdateNotifier(ILog log, string name) : base(log, "Update", name) { }

        #region IDisposable Members

        private bool disposed;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Log.Info($"{Category} {Name} complete after {Stopwatch.ToTotalSeconds("F1")} s.");
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}

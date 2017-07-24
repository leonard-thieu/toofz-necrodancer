using System;
using System.Diagnostics;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public abstract class NotifierBase : IDisposable
    {
        protected NotifierBase(ILog log, string category, string name)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            Log = log;
            Category = category;
            Name = name;

            Log.Debug($"Start {Category} {Name}");
        }

        protected ILog Log { get; }
        protected string Category { get; }
        protected string Name { get; }

        public Stopwatch Stopwatch { get; } = Stopwatch.StartNew();

        #region IDisposable Members

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Log.Debug($"End {Category} {Name}");
            }

            disposed = true;
        }

        #endregion
    }
}

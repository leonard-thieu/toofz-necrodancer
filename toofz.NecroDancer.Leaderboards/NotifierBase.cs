using System;
using System.Diagnostics;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public abstract class NotifierBase : IDisposable
    {
        protected NotifierBase(ILog log, string category, string name)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            Category = category;
            Name = name;

            Log.Debug($"Start {Category} {Name}");
        }

        protected ILog Log { get; }
        protected string Category { get; }
        protected string Name { get; }

        public Stopwatch Stopwatch { get; } = Stopwatch.StartNew();

        #region IDisposable Members

        bool disposed;

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

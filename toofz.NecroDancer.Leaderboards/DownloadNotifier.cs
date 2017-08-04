using System;
using System.Diagnostics;
using System.Threading;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class DownloadNotifier : NotifierBase
    {
        #region Static Members

        static readonly string[] SizeSuffixes = { "bytes", "kB", "MB", "GB" };

        // http://stackoverflow.com/a/14488941/414137
        static string SizeSuffix(long value)
        {
            if (value < 0)
                return "-" + SizeSuffix(-value);
            if (value == 0)
                return "0 bytes";

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:F1} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        static readonly string[] RateSuffixes = { "Bps", "kBps", "MBps", "GBps" };

        static string RateSuffix(double value)
        {
            if (value < 0)
                return "-" + RateSuffix(-value);
            if (value == 0)
                return "0 Bps";

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedRate = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:F1} {1}", adjustedRate, RateSuffixes[mag]);
        }

        #endregion

        public DownloadNotifier(ILog log, string name) : base("Download", log, name)
        {
            Progress = new ActionProgress<long>(r => Interlocked.Add(ref totalBytes, r));
        }

        long totalBytes;

        public IProgress<long> Progress { get; }
        public long TotalBytes => totalBytes;

        #region IDisposable Members

        bool disposed;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                var size = SizeSuffix(totalBytes);
                var time = Stopwatch.Elapsed.TotalSeconds;
                var rate = RateSuffix(totalBytes / time);

                Log.Info($"{Category} {Name} complete -- {size} over {time.ToString("F1")} s ({rate}).");
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}

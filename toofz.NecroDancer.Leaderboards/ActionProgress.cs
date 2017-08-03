using System;

namespace toofz.NecroDancer.Leaderboards
{
    public class ActionProgress<T> : IProgress<T>
    {
        public ActionProgress(Action<T> handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        readonly Action<T> handler;

        public void Report(T value)
        {
            handler(value);
        }
    }
}

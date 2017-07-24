using System;

namespace toofz.NecroDancer.Leaderboards
{
    public class ActionProgress<T> : IProgress<T>
    {
        public ActionProgress(Action<T> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            this.handler = handler;
        }

        private readonly Action<T> handler;

        public void Report(T value)
        {
            handler(value);
        }
    }
}

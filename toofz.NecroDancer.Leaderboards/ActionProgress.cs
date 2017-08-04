using System;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Progress provider that wraps a progress delegate.
    /// </summary>
    /// <typeparam name="T">The type of the value passed to the delegate.</typeparam>
    public sealed class ActionProgress<T> : IProgress<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionProgress{T}"/> class with a specific handler.
        /// </summary>
        /// <param name="handler">
        /// The handler to run when <see cref="Report(T)"/> is called.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="handler"/> is null.
        /// </exception>
        public ActionProgress(Action<T> handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler), $"{nameof(handler)} is null.");
        }

        readonly Action<T> handler;

        /// <summary>
        /// Reports a progress update.
        /// </summary>
        /// <param name="value">The value of the updated progress.</param>
        public void Report(T value)
        {
            handler(value);
        }
    }
}

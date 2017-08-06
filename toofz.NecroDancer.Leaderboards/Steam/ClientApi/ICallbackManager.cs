using System;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public interface ICallbackManager
    {
        ISteamClient SteamClient { get; }
        /// <summary>
        /// Blocks the current thread to run all queued callbacks. If no callback is queued,
        /// the method will block for the given timeout.
        /// </summary>
        /// <param name="timeout">The length of time to block.</param>
        void RunWaitAllCallbacks(TimeSpan timeout);
        /// <summary>
        /// Registers the provided <see cref="Action{TCallback}"/> to receive callbacks of type <typeparamref name="TCallback"/>.
        /// </summary>
        /// <typeparam name="TCallback"></typeparam>
        /// <param name="callbackFunc">
        /// The function to invoke with the callback.
        /// </param>
        /// <returns>
        /// An <see cref="IDisposable"/>. Disposing of the return value will unsubscribe the <paramref name="callbackFunc"/>.
        /// </returns>
        IDisposable Subscribe<TCallback>(Action<TCallback> callbackFunc) where TCallback : class, ICallbackMsg;
    }
}

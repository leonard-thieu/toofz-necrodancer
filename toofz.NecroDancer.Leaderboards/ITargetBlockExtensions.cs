using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace toofz.NecroDancer.Leaderboards
{
    static class ITargetBlockExtensions
    {
        public static async Task CheckSendAsync<T>(
            this ITargetBlock<T> target,
            T item,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target), $"{nameof(target)} is null.");

            if (!await target.SendAsync(item, cancellationToken).ConfigureAwait(false))
            {
                throw new InvalidOperationException("Item was declined by target message block.");
            }
        }
    }
}

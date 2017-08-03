using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public interface IUgcHttpClient
    {
        Task<Stream> GetUgcFileAsync(
            string url,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
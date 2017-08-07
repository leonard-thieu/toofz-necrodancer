using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public interface IToofzApiClient : IDisposable
    {
        Task<Players> GetPlayersAsync(
            GetPlayersParams @params = null,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<string> PostPlayersAsync(
            IEnumerable<Leaderboards.Player> players,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<Replays> GetReplaysAsync(
            GetReplaysParams @params = null,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<string> PostReplaysAsync(
            IEnumerable<Leaderboards.Replay> replays,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
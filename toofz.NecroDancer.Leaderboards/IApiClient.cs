using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public interface IApiClient : IDisposable
    {
        Task<IEnumerable<long>> GetMissingReplayIdsAsync(int limit, CancellationToken cancellationToken);
        Task<IEnumerable<long>> GetStaleSteamIdsAsync(int limit, CancellationToken cancellationToken);
        Task<string> PostPlayersAsync(IEnumerable<Player> players, CancellationToken cancellationToken);
        Task<string> PostReplaysAsync(IEnumerable<Replay> replays, CancellationToken cancellationToken);
    }
}
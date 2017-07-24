using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public interface ILeaderboardsHttpClient : IDisposable
    {
        string SteamWebApiKey { get; set; }

        Task<IEnumerable<LeaderboardHeader>> GetLeaderboardHeadersAsync(CancellationToken cancellationToken);
        Task<Leaderboard> GetLeaderboardAsync(LeaderboardHeader header, IProgress<long> progress, CancellationToken cancellationToken);
        Task<IEnumerable<Player>> GetPlayersAsync(IEnumerable<long> steamIds, CancellationToken cancellationToken);
        Task<IEnumerable<ReplayContext>> GetReplaysAsync(IEnumerable<long> ugcIds, CancellationToken cancellationToken);
    }
}

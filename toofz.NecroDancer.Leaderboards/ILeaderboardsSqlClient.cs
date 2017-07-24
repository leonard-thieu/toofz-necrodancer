using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public interface ILeaderboardsSqlClient
    {
        Task SaveChangesAsync(IEnumerable<Leaderboard> leaderboards, CancellationToken cancellationToken);
        Task SaveChangesAsync(IEnumerable<Entry> entries);
        Task SaveChangesAsync(IEnumerable<DailyLeaderboard> leaderboards, CancellationToken cancellationToken);
        Task SaveChangesAsync(IEnumerable<DailyEntry> entries, CancellationToken cancellationToken);
        Task SaveChangesAsync(IEnumerable<Replay> replays, bool updateOnMatch, CancellationToken cancellationToken);
        Task SaveChangesAsync(IEnumerable<Player> players, bool updateOnMatch, CancellationToken cancellationToken);
    }
}

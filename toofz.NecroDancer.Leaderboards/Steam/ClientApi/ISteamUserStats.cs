using System.Threading.Tasks;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public interface ISteamUserStats
    {
        /// <summary>
        /// Asks the Steam back-end for a leaderboard by name for a given appid. Results
        /// are returned in a <see cref="IFindOrCreateLeaderboardCallback"/>.
        /// </summary>
        /// <param name="appId">The AppID to request a leaderboard for.</param>
        /// <param name="name">Name of the leaderboard to request.</param>
        Task<IFindOrCreateLeaderboardCallback> FindLeaderboard(uint appId, string name);
        /// <summary>
        /// Asks the Steam back-end for a set of rows in the leaderboard. Results are returned
        /// in a <see cref="ILeaderboardEntriesCallback"/>.
        /// </summary>
        /// <param name="appId">The AppID to request leaderboard rows for.</param>
        /// <param name="id">ID of the leaderboard to view.</param>
        /// <param name="rangeStart">Range start or 0.</param>
        /// <param name="rangeEnd">Range end or max leaderboard entries.</param>
        /// <param name="dataRequest">Type of request.</param>
        Task<ILeaderboardEntriesCallback> GetLeaderboardEntries(uint appId, int id, int rangeStart, int rangeEnd, ELeaderboardDataRequest dataRequest);
    }
}

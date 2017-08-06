using System;
using System.Threading.Tasks;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    sealed class SteamUserStatsAdapter : ISteamUserStats
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SteamUserStatsAdapter"/> class.
        /// </summary>
        /// <param name="steamUserStats">
        /// The <see cref="SteamUserStats"/> instance to wrap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="steamUserStats"/> is null.
        /// </exception>
        public SteamUserStatsAdapter(SteamUserStats steamUserStats)
        {
            this.steamUserStats = steamUserStats ?? throw new ArgumentNullException(nameof(steamUserStats), $"{nameof(steamUserStats)} is null.");
        }

        readonly SteamUserStats steamUserStats;

        /// <summary>
        /// Asks the Steam back-end for a leaderboard by name for a given appid. Results
        /// are returned in a <see cref="IFindOrCreateLeaderboardCallback"/>.
        /// </summary>
        /// <param name="appId">The AppID to request a leaderboard for.</param>
        /// <param name="name">Name of the leaderboard to request.</param>
        public async Task<IFindOrCreateLeaderboardCallback> FindLeaderboard(uint appId, string name)
        {
            var leaderboard = await steamUserStats
                .FindLeaderboard(appId, name)
                .ToTask()
                .ConfigureAwait(false);

            return new FindOrCreateLeaderboardCallbackAdapter(leaderboard);
        }

        /// <summary>
        /// Asks the Steam back-end for a set of rows in the leaderboard. Results are returned
        /// in a <see cref="LeaderboardEntriesCallback"/>.
        /// </summary>
        /// <param name="appId">The AppID to request leaderboard rows for.</param>
        /// <param name="id">ID of the leaderboard to view.</param>
        /// <param name="rangeStart">Range start or 0.</param>
        /// <param name="rangeEnd">Range end or max leaderboard entries.</param>
        /// <param name="dataRequest">Type of request.</param>
        public async Task<ILeaderboardEntriesCallback> GetLeaderboardEntries(uint appId, int id, int rangeStart, int rangeEnd, ELeaderboardDataRequest dataRequest)
        {
            var leaderboardEntries = await steamUserStats
                .GetLeaderboardEntries(appId, id, rangeStart, rangeEnd, dataRequest)
                .ToTask()
                .ConfigureAwait(false);

            return new LeaderboardEntriesCallbackAdapter(leaderboardEntries);
        }
    }
}

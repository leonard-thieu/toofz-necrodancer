using System.Collections.ObjectModel;
using SteamKit2;
using static SteamKit2.SteamUserStats.LeaderboardEntriesCallback;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public interface ILeaderboardEntriesCallback : ICallbackMsg
    {
        /// <summary>
        /// Gets the result of the request.
        /// </summary>
        EResult Result { get; }
        /// <summary>
        /// How many entires there are for requested leaderboard.
        /// </summary>
        int EntryCount { get; }
        /// <summary>
        /// Gets the list of leaderboard entries this response contains.
        /// </summary>
        ReadOnlyCollection<LeaderboardEntry> Entries { get; }
    }
}

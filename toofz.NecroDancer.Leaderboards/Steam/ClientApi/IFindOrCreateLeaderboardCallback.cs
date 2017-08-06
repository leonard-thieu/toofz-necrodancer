using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public interface IFindOrCreateLeaderboardCallback : ICallbackMsg
    {
        /// <summary>
        /// Gets the result of the request.
        /// </summary>
        EResult Result { get; }
        /// <summary>
        /// Leaderboard ID.
        /// </summary>
        int ID { get; }
        /// <summary>
        /// How many entires there are for requested leaderboard.
        /// </summary>
        int EntryCount { get; }
        /// <summary>
        /// Sort method to use for this leaderboard.
        /// </summary>
        ELeaderboardSortMethod SortMethod { get; }
        /// <summary>
        /// Display type for this leaderboard.
        /// </summary>
        ELeaderboardDisplayType DisplayType { get; }
    }
}

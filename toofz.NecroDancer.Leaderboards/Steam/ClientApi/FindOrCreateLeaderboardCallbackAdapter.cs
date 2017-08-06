using System;
using SteamKit2;
using static SteamKit2.SteamUserStats;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    sealed class FindOrCreateLeaderboardCallbackAdapter : IFindOrCreateLeaderboardCallback
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindOrCreateLeaderboardCallbackAdapter"/>.
        /// </summary>
        /// <param name="leaderboard">
        /// The <see cref="FindOrCreateLeaderboardCallback"/> to wrap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="leaderboard"/> is null.
        /// </exception>
        public FindOrCreateLeaderboardCallbackAdapter(FindOrCreateLeaderboardCallback leaderboard)
        {
            this.leaderboard = leaderboard ?? throw new ArgumentNullException(nameof(leaderboard), $"{nameof(leaderboard)} is null.");
        }

        readonly FindOrCreateLeaderboardCallback leaderboard;

        /// <summary>
        /// Gets the result of the request.
        /// </summary>
        public EResult Result => leaderboard.Result;

        /// <summary>
        /// Leaderboard ID.
        /// </summary>
        public int ID => leaderboard.ID;

        /// <summary>
        /// How many entires there are for requested leaderboard.
        /// </summary>
        public int EntryCount => leaderboard.EntryCount;

        /// <summary>
        /// Sort method to use for this leaderboard.
        /// </summary>
        public ELeaderboardSortMethod SortMethod => leaderboard.SortMethod;

        /// <summary>
        /// Display type for this leaderboard.
        /// </summary>
        public ELeaderboardDisplayType DisplayType => leaderboard.DisplayType;

        /// <summary>
        /// The <see cref="ICallbackMsg.JobID"/> that this callback is associated with. If there
        /// is no job associated, then this will be <see cref="JobID.Invalid"/>
        /// </summary>
        public JobID JobID
        {
            get => leaderboard.JobID;
            set => leaderboard.JobID = value;
        }
    }
}

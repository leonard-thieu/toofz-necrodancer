using System;
using System.Collections.ObjectModel;
using SteamKit2;
using static SteamKit2.SteamUserStats;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    sealed class LeaderboardEntriesCallbackAdapter : ILeaderboardEntriesCallback
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderboardEntriesCallbackAdapter"/> class.
        /// </summary>
        /// <param name="leaderboardEntries">
        /// The <see cref="LeaderboardEntriesCallback"/> instance to wrap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="leaderboardEntries"/> is null.
        /// </exception>
        public LeaderboardEntriesCallbackAdapter(LeaderboardEntriesCallback leaderboardEntries)
        {
            this.leaderboardEntries = leaderboardEntries ?? throw new ArgumentNullException(nameof(leaderboardEntries), $"{nameof(leaderboardEntries)} is null.");
        }

        readonly LeaderboardEntriesCallback leaderboardEntries;

        /// <summary>
        /// Gets the result of the request.
        /// </summary>
        public EResult Result => leaderboardEntries.Result;

        /// <summary>
        /// How many entires there are for requested leaderboard.
        /// </summary>
        public int EntryCount => leaderboardEntries.EntryCount;

        /// <summary>
        /// Gets the list of leaderboard entries this response contains.
        /// </summary>
        public ReadOnlyCollection<LeaderboardEntriesCallback.LeaderboardEntry> Entries => leaderboardEntries.Entries;

        /// <summary>
        /// The <see cref="ICallbackMsg.JobID"/> that this callback is associated with. If there
        /// is no job associated, then this will be <see cref="JobID.Invalid"/>
        /// </summary>
        public JobID JobID
        {
            get => leaderboardEntries.JobID;
            set => leaderboardEntries.JobID = value;
        }
    }
}

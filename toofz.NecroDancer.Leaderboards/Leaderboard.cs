using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer leaderboard.
    /// </summary>
    public sealed class Leaderboard
    {
        /// <summary>
        /// A value that uniquely identifies the leaderboard.
        /// </summary>
        public int LeaderboardId { get; set; }
        /// <summary>
        /// The total number of entries for the leaderboard. This value is provided by Steam.
        /// </summary>
        public int EntriesCount { get; set; }

        /// <summary>
        /// The leaderboard's collection of entries.
        /// </summary>
        public List<Entry> Entries { get; } = new List<Entry>();

        /// <summary>
        /// The last time that the leaderboard was updated.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// The ID of the character associated with the leaderboard.
        /// </summary>
        public int CharacterId { get; set; }
        /// <summary>
        /// The ID of the run associated with the leaderboard.
        /// </summary>
        public int RunId { get; set; }
        /// <summary>
        /// The date associated with the leaderboard if it is a daily. This value is null for leaderboards 
        /// that are not dailies.
        /// </summary>
        public DateTime? Date { get; set; }
    }
}

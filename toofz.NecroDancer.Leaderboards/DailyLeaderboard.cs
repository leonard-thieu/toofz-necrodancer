using System;
using System.Collections.Generic;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer daily leaderboard.
    /// </summary>
    public sealed class DailyLeaderboard
    {
        /// <summary>
        /// A value that uniquely identifies the leaderboard.
        /// </summary>
        public int LeaderboardId { get; set; }
        /// <summary>
        /// The leaderboard's collection of entries.
        /// </summary>
        public List<DailyEntry> Entries { get; } = new List<DailyEntry>();
        /// <summary>
        /// The last time that the leaderboard was updated.
        /// </summary>
        public DateTime LastUpdate { get; set; }
        /// <summary>
        /// The date associated with the daily leaderboard.
        /// </summary>
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public bool IsProduction { get; set; }
    }
}

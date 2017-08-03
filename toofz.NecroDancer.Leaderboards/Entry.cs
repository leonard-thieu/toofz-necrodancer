namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// A Crypt of the NecroDancer leaderboard entry.
    /// </summary>
    public sealed class Entry
    {
        /// <summary>
        /// The ID of the leaderboard associated with the entry.
        /// </summary>
        public int LeaderboardId { get; set; }
        /// <summary>
        /// The leaderboard associated with the entry.
        /// </summary>
        public Leaderboard Leaderboard { get; set; }
        /// <summary>
        /// The rank of the entry relative to other entries on the leaderboard.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// The Steam ID of the player who submitted the entry.
        /// </summary>
        public long SteamId { get; set; }
        /// <summary>
        /// The player who submitted the entry.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The ID of the replay associated with the entry. This may be null if there was no replay submitted 
        /// with the entry (UGCID = -1).
        /// </summary>
        public long? ReplayId { get; set; }

        /// <summary>
        /// The score achieved by the entry.
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// The zone that the player was in when the run ended.
        /// </summary>
        public int Zone { get; set; }
        /// <summary>
        /// The level that the player was in when the run ended.
        /// </summary>
        public int Level { get; set; }
    }
}

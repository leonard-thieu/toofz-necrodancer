namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer replay.
    /// </summary>
    public sealed class Replay
    {
        /// <summary>
        /// A unique value provided by Steam to identify the replay.
        /// </summary>
        public long ReplayId { get; set; }

        public int? ErrorCode { get; set; }

        /// <summary>
        /// The seed of the replay.
        /// </summary>
        public int? Seed { get; set; }
        /// <summary>
        /// The version of the replay format.
        /// </summary>
        public int? Version { get; set; }
        /// <summary>
        /// The name of the entity that killed the player. This may be null.
        /// </summary>
        public string KilledBy { get; set; }

        public string FileName => Version != null ? $"{Version}_{ReplayId}.dat" : $"UNKNOWN_{ReplayId}.dat";
    }
}

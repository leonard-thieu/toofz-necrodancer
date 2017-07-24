using System;

namespace toofz.NecroDancer.Replays
{
    /// <summary>
    /// Represents the header of a Crypt of the NecroDancer replay.
    /// </summary>
    /// <seealso cref="ReplayData"/>
    /// <seealso cref="ReplaySerializer"/>
    public sealed class Header
    {
        public string KilledBy { get; set; }

        /// <summary>
        /// Indicates if the replay was retrieved from a remote source.
        /// </summary>
        public bool IsRemote { get; set; }
        /// <summary>
        /// The replay version number.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// The zone that the replay starts in.
        /// </summary>
        public int StartZone { get; set; }
        /// <summary>
        /// The amount of coins that the player starts with.
        /// </summary>
        public int StartCoins { get; set; }
        /// <summary>
        /// Indicates if the player starts with a Broadsword.
        /// </summary>
        public bool HasBroadsword { get; set; }
        /// <summary>
        /// Indicates if the replay is an All Zones mode replay.
        /// </summary>
        public bool IsHardcore { get; set; }
        /// <summary>
        /// Indicates if the replay is a Daily mode replay.
        /// </summary>
        public bool IsDaily { get; set; }
        /// <summary>
        /// Indicates if the replay is a Dance Pad mode replay. 
        /// </summary>
        public bool IsDancePadMode { get; set; }
        /// <summary>
        /// Indicates if the replay is a Seeded mode replay.
        /// </summary>
        public bool IsSeeded { get; set; }
        /// <summary>
        /// The duration of the replay.
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// The number of levels in the replay.
        /// </summary>
        public int LevelCount { get; set; }

        public int Run { get; set; }
        public int Unknown0 { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Steam user.
    /// </summary>
    public sealed class Player
    {
        /// <summary>
        /// The player's Steam ID.
        /// </summary>
        public long SteamId { get; set; }

        public bool? Exists { get; set; }

        /// <summary>
        /// The player's display name.
        /// </summary>
        public string Name { get; set; }

        public string Avatar { get; set; }

        /// <summary>
        /// The last time the player's information was updated.
        /// </summary>
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Entries that the player has submitted.
        /// </summary>
        public List<Entry> Entries { get; } = new List<Entry>();
    }
}

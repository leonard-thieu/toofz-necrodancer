using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("steamid")]
        public long SteamId { get; set; }

        public bool? Exists { get; set; }

        /// <summary>
        /// The player's display name.
        /// </summary>
        [JsonProperty("personaname")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
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

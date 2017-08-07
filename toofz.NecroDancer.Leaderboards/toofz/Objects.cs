using System;
using System.Collections.Generic;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    /// <summary>
    /// A page of Steam players.
    /// </summary>
    public class Players
    {
        /// <summary>
        /// Total number of players.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of players.
        /// </summary>
        public IEnumerable<Player> players { get; set; } = new List<Player>();
    }

    /// <summary>
    /// A Steam player.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The player's Steam ID.
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// The player's display name.
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// The time (in UTC) that the player's data was retrieved at.
        /// </summary>
        public DateTime? updated_at { get; set; }
        /// <summary>
        /// The URL of the player's avatar.
        /// </summary>
        public string avatar { get; set; }
    }

    public class Replay
    {
        public long id { get; set; }
        public int? error { get; set; }
        public int? seed { get; set; }
        public int? version { get; set; }
        public string killed_by { get; set; }
    }

    public class Replays
    {
        public int total { get; set; }
        public IEnumerable<Replay> replays { get; set; } = new List<Replay>();
    }

    /// <summary>
    /// Represents the response of a bulk store operation.
    /// </summary>
    public class BulkStore
    {
        /// <summary>
        /// The number of rows affected.
        /// </summary>
        public int rows_affected { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer item.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The item's name.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The item's display name.
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// The slot that the item can be equipped in.
        /// </summary>
        public string slot { get; set; }
        /// <summary>
        /// The item's unlock cost.
        /// </summary>
        public int? unlock { get; set; }
        /// <summary>
        /// The item's base price.
        /// </summary>
        public int? cost { get; set; }
    }

    /// <summary>
    /// A page of Crypt of the NecroDancer items.
    /// </summary>
    public class Items
    {
        /// <summary>
        /// The total number of items in the result set.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of items.
        /// </summary>
        public IEnumerable<Item> items { get; set; }
    }

    /// <summary>
    /// A Crypt of the NecroDancer enemy.
    /// </summary>
    public class Enemy
    {
        /// <summary>
        /// The enemy's element name.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The enemy's type.
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// The enemy's display name.
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// The enemy's health.
        /// </summary>
        public int health { get; set; }
        /// <summary>
        /// The amount of damage that the enemy does per attack.
        /// </summary>
        public int damage { get; set; }
        /// <summary>
        /// The enemy's beats per move.
        /// </summary>
        public int beats_per_move { get; set; }
        /// <summary>
        /// The base amount of coins the enemy drops when killed.
        /// </summary>
        public int drops { get; set; }
    }

    /// <summary>
    /// A page of Crypt of the NecroDancer enemies.
    /// </summary>
    public class Enemies
    {
        /// <summary>
        /// The total number of enemies in the result set.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of enemies.
        /// </summary>
        public IEnumerable<Enemy> enemies { get; set; }
    }

    /// <summary>
    /// A Crypt of the NecroDancer leaderboard.
    /// </summary>
    public class Leaderboard
    {
        /// <summary>
        /// Steam lbid
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer product (e.g. classic, amplified)
        /// </summary>
        public string product { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer character (e.g. all-characters, cadence, story-mode)
        /// </summary>
        public string character { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer mode (e.g. standard, hard, no-return)
        /// </summary>
        public string mode { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer run (e.g. score, seeded-speed, deathless)
        /// </summary>
        public string run { get; set; }
        /// <summary>
        /// Display name for the leaderboard.
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// Time that the leaderboard was last updated at (in UTC)
        /// </summary>
        public DateTime updated_at { get; set; }
        /// <summary>
        /// Total number of entries
        /// </summary>
        public int total { get; set; }
    }

    /// <summary>
    /// A page of Crypt of the NecroDancer leaderboards.
    /// </summary>
    public class Leaderboards
    {
        /// <summary>
        /// Total number of leaderboards.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of leaderboards.
        /// </summary>
        public IEnumerable<Leaderboard> leaderboards { get; set; }
    }

    /// <summary>
    /// A Crypt of the NecroDancer daily leaderboard.
    /// </summary>
    public class DailyLeaderboard
    {
        /// <summary>
        /// Steam lbid
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// The date of the daily leaderboard.
        /// </summary>
        public DateTime date { get; set; }
        /// <summary>
        /// Time that the leaderboard was last updated at (in UTC)
        /// </summary>
        public DateTime updated_at { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer product (e.g. classic, amplified)
        /// </summary>
        public string product { get; set; }
        /// <summary>
        /// Indicates if the daily leaderboard is a production leaderboard.
        /// </summary>
        public bool production { get; set; }
    }

    /// <summary>
    /// A page of Crypt of the NecroDancer daily leaderboards.
    /// </summary>
    public class DailyLeaderboards
    {
        /// <summary>
        /// Total number of daily leaderboards.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of daily leaderboards.
        /// </summary>
        public IEnumerable<DailyLeaderboard> leaderboards { get; set; }
    }

    /// <summary>
    /// A Steam player.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The player's Steam ID.
        /// </summary>
        public string id { get; set; }
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
        public IEnumerable<Player> players { get; set; }
    }

    /// <summary>
    /// The zone and level that the entry ends on.
    /// </summary>
    public class End
    {
        /// <summary>
        /// The zone that the entry ends on.
        /// </summary>
        public int zone { get; set; }
        /// <summary>
        /// The level that the entry ends on.
        /// </summary>
        public int level { get; set; }
    }

    /// <summary>
    /// A Crypt of the NecroDancer leaderboard entry.
    /// </summary>
    public class Entry
    {
        /// <summary>
        /// The leaderboard that contains the entry.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Leaderboard leaderboard { get; set; }
        /// <summary>
        /// The player that submitted the entry.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Player player { get; set; }
        /// <summary>
        /// The rank of the entry on the leaderboard.
        /// </summary>
        public int rank { get; set; }
        /// <summary>
        /// The raw score value.
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// The zone and level that the entry ends on.
        /// </summary>
        public End end { get; set; }
        /// <summary>
        /// The entity that killed the player. This may be null if the replay has not been parsed yet or 
        /// there was an error parsing the replay.
        /// </summary>
        public string killed_by { get; set; }
        /// <summary>
        /// Version of the entry's replay. This may be null if the replay has not been parsed yet or 
        /// there was an error parsing the replay.
        /// </summary>
        public int? version { get; set; }
    }

    /// <summary>
    /// A page of Crypt of the NecroDancer leaderboard entries.
    /// </summary>
    public class LeaderboardEntries
    {
        /// <summary>
        /// The Crypt of the NecroDancer leaderboard.
        /// </summary>
        public Leaderboard leaderboard { get; set; }
        /// <summary>
        /// Total number of leaderboard entries.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of leaderboard entries.
        /// </summary>
        public IEnumerable<Entry> entries { get; set; }
    }

    /// <summary>
    /// A page of Crypt of the NecroDancer daily leaderboard entries.
    /// </summary>
    public class DailyLeaderboardEntries
    {
        /// <summary>
        /// The Crypt of the NecroDancer daily leaderboard.
        /// </summary>
        public DailyLeaderboard leaderboard { get; set; }
        /// <summary>
        /// Total number of daily leaderboard entries.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of daily leaderboard entries.
        /// </summary>
        public IEnumerable<Entry> entries { get; set; }
    }

    public class PlayerEntries
    {
        /// <summary>
        /// The Steam player.
        /// </summary>
        public Player player { get; set; }
        /// <summary>
        /// Total number of leaderboard entries.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of leaderboard entries.
        /// </summary>
        public IEnumerable<Entry> entries { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// Represents a Steam player.
    /// </summary>
    public sealed class PlayerModel
    {
        /// <summary>
        /// The player's Steam ID.
        /// </summary>
        [Required]
        public long? SteamId { get; set; }

        /// <summary>
        /// Indicates if a player exists.
        /// </summary>
        [Required]
        public bool? Exists { get; set; }

        /// <summary>
        /// The player's display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The last time the player's information was updated.
        /// </summary>
        [Required]
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// The URL of the player's avatar.
        /// </summary>
        public string Avatar { get; set; }
    }
}
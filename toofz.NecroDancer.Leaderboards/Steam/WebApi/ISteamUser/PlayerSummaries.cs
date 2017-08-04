using System.Collections.Generic;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser
{
    public sealed class PlayerSummaries
    {
        [JsonProperty("response")]
        public PlayerSummariesResponse Response { get; set; }
    }

    public sealed class PlayerSummariesResponse
    {
        /// <summary>
        /// A list of profile objects. Contained information varies depending on whether or not the user has their profile 
        /// set to Friends only or Private.
        /// </summary>
        [JsonProperty("players")]
        public List<PlayerSummary> Players { get; } = new List<PlayerSummary>();
    }

    /// <summary>
    /// Basic profile information.
    /// </summary>
    public sealed class PlayerSummary
    {
        /// <summary>
        /// 64-bit SteamID of the user
        /// </summary>
        [JsonProperty("steamid")]
        public long SteamId { get; set; }
        /// <summary>
        /// This represents whether the profile is visible or not, and if it is visible, why you are allowed to see it. 
        /// Note that because this WebAPI does not use authentication, there are only two possible values returned: 
        ///   1 - The profile is not visible to you (Private, Friends Only, etc).
        ///   3 - The profile is "Public", and the data is visible.
        /// Mike Blaszczak's post on Steam forums says, 
        ///   The community visibility state this API returns is different than the privacy state. It's the effective visibility state from 
        ///   the account making the request to the account being viewed given the requesting account's relationship to the viewed account.
        /// </summary>
        [JsonProperty("communityvisibilitystate")]
        public int CommunityVisibilityState { get; set; }
        /// <summary>
        /// If set, indicates the user has a community profile configured (will be set to '1')
        /// </summary>
        [JsonProperty("profilestate")]
        public int ProfileState { get; set; }
        /// <summary>
        /// The player's persona name (display name)
        /// </summary>
        [JsonProperty("personaname")]
        public string PersonaName { get; set; }
        /// <summary>
        /// The last time the user was online, in unix time.
        /// </summary>
        [JsonProperty("lastlogoff")]
        public int LastLogOff { get; set; }
        /// <summary>
        /// The full URL of the player's Steam Community profile.
        /// </summary>
        [JsonProperty("profileurl")]
        public string ProfileUrl { get; set; }
        /// <summary>
        /// The full URL of the player's 32x32px avatar. If the user hasn't configured an avatar, this will be the default ? avatar.
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        /// <summary>
        /// The full URL of the player's 64x64px avatar. If the user hasn't configured an avatar, this will be the default ? avatar.
        /// </summary>
        [JsonProperty("avatarmedium")]
        public string AvatarMedium { get; set; }
        /// <summary>
        /// The full URL of the player's 184x184px avatar. If the user hasn't configured an avatar, this will be the default ? avatar.
        /// </summary>
        [JsonProperty("avatarfull")]
        public string AvatarFull { get; set; }
        /// <summary>
        /// The user's current status.
        ///   0 - Offline
        ///   1 - Online
        ///   2 - Busy
        ///   3 - Away
        ///   4 - Snooze
        ///   5 - Looking to Trade
        ///   6 - Looking to Play
        /// If the player's profile is private, this will always be "0", except if the user has set their status to Looking to Trade or Looking to Play, 
        /// because a bug makes those status appear even if the profile is private.
        /// </summary>
        [JsonProperty("personastate")]
        public int PersonaState { get; set; }
        /// <summary>
        /// The player's "Real Name", if they have set it.
        /// </summary>
        [JsonProperty("realname")]
        public string RealName { get; set; }
        /// <summary>
        /// The player's primary group, as configured in their Steam Community profile.
        /// </summary>
        [JsonProperty("primaryclanid")]
        public ulong PrimaryClanId { get; set; }
        /// <summary>
        /// The time the player's account was created.
        /// </summary>
        [JsonProperty("timecreated")]
        public int TimeCreated { get; set; }
        [JsonProperty("personastateflags")]
        public int PersonaStateFlags { get; set; }
        /// <summary>
        /// If set on the user's Steam Community profile, The user's country of residence, 2-character ISO country code
        /// </summary>
        [JsonProperty("loccountrycode")]
        public string LocCountryCode { get; set; }
        /// <summary>
        /// If set on the user's Steam Community profile, The user's state of residence
        /// </summary>
        [JsonProperty("locstatecode")]
        public string LocStateCode { get; set; }
        /// <summary>
        /// An internal code indicating the user's city of residence. A future update will provide this data in a more useful way.
        /// </summary>
        [JsonProperty("loccityid")]
        public int LocCityId { get; set; }
    }
}

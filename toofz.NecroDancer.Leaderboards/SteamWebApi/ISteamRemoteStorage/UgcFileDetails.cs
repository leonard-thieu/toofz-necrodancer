using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards.SteamWebApi.ISteamRemoteStorage
{
    public sealed class UgcFileDetails
    {
        /// <summary>
        /// UGC file information.
        /// </summary>
        [JsonProperty("data")]
        public UgcFileDetailsData Data { get; set; }
    }

    /// <summary>
    /// UGC file information.
    /// </summary>
    public sealed class UgcFileDetailsData
    {
        /// <summary>
        /// Path to the file along with its name
        /// </summary>
        [JsonProperty("filename")]
        public string FileName { get; set; }
        /// <summary>
        /// URL to the file
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        /// <summary>
        /// Size of the file
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; set; }
    }
}

using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class OAuth2AccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty(".issued")]
        public string Issued { get; set; }
        [JsonProperty(".expires")]
        public string Expires { get; set; }
    }
}

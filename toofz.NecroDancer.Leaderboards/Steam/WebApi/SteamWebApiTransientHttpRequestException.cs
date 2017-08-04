using System;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi
{
    public sealed class SteamWebApiTransientHttpRequestException : HttpRequestStatusException
    {
        /// <summary>
        /// Initializes an instance of the <see cref="SteamWebApiTransientHttpRequestException"/> class.
        /// </summary>
        public SteamWebApiTransientHttpRequestException() { }

        /// <summary>
        /// Initializes an instance of the <see cref="SteamWebApiTransientHttpRequestException"/> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        public SteamWebApiTransientHttpRequestException(string message) : base(message) { }

        /// <summary>
        /// Initializes an instance of the <see cref="SteamWebApiTransientHttpRequestException"/> class
        /// with a specific message that describes the current exception and an inner exception.
        /// </summary>
        public SteamWebApiTransientHttpRequestException(string message, Exception inner) : base(message, inner) { }
    }
}

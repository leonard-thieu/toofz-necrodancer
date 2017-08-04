using System;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public class SteamClientApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiException"/> class.
        /// </summary>
        public SteamClientApiException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiException"/> class with a specified error 
        /// message.
        /// </summary>
        public SteamClientApiException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiException"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        public SteamClientApiException(string message, Exception inner) : base(message, inner) { }

        public EResult? Result { get; set; }
    }
}

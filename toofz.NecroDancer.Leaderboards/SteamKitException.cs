using System;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards
{
    public class SteamKitException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SteamKitException"/> class.
        /// </summary>
        public SteamKitException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamKitException"/> class with a specified error 
        /// message.
        /// </summary>
        public SteamKitException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamKitException"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        public SteamKitException(string message, Exception inner) : base(message, inner) { }

        public EResult? Result { get; set; }
    }
}

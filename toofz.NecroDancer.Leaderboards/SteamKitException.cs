using System;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards
{
    public class SteamKitException : Exception
    {
        public SteamKitException() { }

        public SteamKitException(string message) : base(message) { }

        public SteamKitException(string message, Exception inner) : base(message, inner) { }

        public EResult? Result { get; set; }
    }
}

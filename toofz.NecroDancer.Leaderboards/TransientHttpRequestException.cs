using System;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class TransientHttpRequestException : HttpRequestStatusException
    {
        public TransientHttpRequestException() { }

        public TransientHttpRequestException(string message) : base(message) { }

        public TransientHttpRequestException(string message, Exception inner) : base(message, inner) { }
    }
}

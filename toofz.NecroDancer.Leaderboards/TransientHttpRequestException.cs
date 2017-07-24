using System;
using System.Net;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class TransientHttpRequestException : Exception
    {
        public TransientHttpRequestException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}

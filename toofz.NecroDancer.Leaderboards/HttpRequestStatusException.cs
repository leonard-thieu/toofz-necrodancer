using System;
using System.Net;
using System.Net.Http;

namespace toofz.NecroDancer.Leaderboards
{
    public class HttpRequestStatusException : HttpRequestException
    {
        public HttpRequestStatusException() { }

        public HttpRequestStatusException(string message)
            : base(message) { }

        public HttpRequestStatusException(string message, Exception inner)
            : base(message, inner) { }

        public HttpStatusCode StatusCode { get; set; }
    }
}

using System;
using System.Net;
using System.Net.Http;

namespace toofz.NecroDancer.Leaderboards
{
    public class HttpRequestStatusException : HttpRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestStatusException"/> class.
        /// </summary>
        public HttpRequestStatusException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestStatusException"/> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        public HttpRequestStatusException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestStatusException"/> class
        /// with a specific message that describes the current exception and an inner exception.
        /// </summary>
        public HttpRequestStatusException(string message, Exception inner)
            : base(message, inner) { }

        /// <summary>
        /// The status code returned by the server for the request.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        public Uri RequestUri { get; set; }
        public string ResponseContent { get; set; }
    }
}

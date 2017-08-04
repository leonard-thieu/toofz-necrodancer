using System;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class TransientHttpRequestException : HttpRequestStatusException
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TransientHttpRequestException"/> class.
        /// </summary>
        public TransientHttpRequestException() { }

        /// <summary>
        /// Initializes an instance of the <see cref="TransientHttpRequestException"/> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        public TransientHttpRequestException(string message) : base(message) { }

        /// <summary>
        /// Initializes an instance of the <see cref="TransientHttpRequestException"/> class
        /// with a specific message that describes the current exception and an inner exception.
        /// </summary>
        public TransientHttpRequestException(string message, Exception inner) : base(message, inner) { }
    }
}

using System;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class HttpError
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }

        public HttpErrorException ToHttpErrorException()
        {
            return new HttpErrorException(Message, StackTrace)
            {
                ExceptionMessage = ExceptionMessage,
                ExceptionType = ExceptionType,
            };
        }
    }

    public sealed class HttpErrorException : Exception
    {
        public HttpErrorException(string message, string stackTrace) : base(message)
        {
            this.stackTrace = stackTrace;
        }

        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }

        readonly string stackTrace;
        public override string StackTrace => stackTrace;
    }
}

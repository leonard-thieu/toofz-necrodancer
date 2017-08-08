using System;
using System.Collections.Generic;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class HttpError
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }

        public override string ToString()
        {
            var lines = new List<string>();

            if (Message != null)
                lines.Add($"Message={Message}");
            if (ExceptionMessage != null)
                lines.Add($"ExceptionMessage={ExceptionMessage}");
            if (ExceptionType != null)
                lines.Add($"ExceptionType={ExceptionType}");
            if (StackTrace != null)
                lines.Add($"StackTrace={StackTrace}");

            lines.Insert(0, nameof(HttpError));

            return string.Join(Environment.NewLine, lines);
        }

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

using System;
using System.Runtime.Serialization;

namespace toofz
{
    /// <summary>
    /// An exception that is thrown when a connection string is not found.
    /// </summary>
    [Serializable]
    public class ConnectionStringNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringNotFoundException" /> class.
        /// </summary>
        public ConnectionStringNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringNotFoundException" /> class with the specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConnectionStringNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringNotFoundException" /> class with a specified error message and 
        /// a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) 
        /// if no inner exception is specified.
        /// </param>
        public ConnectionStringNotFoundException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringNotFoundException" /> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext" /> that contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info" /> is null.
        /// </exception>
        /// <exception cref="SerializationException">
        /// The class name is null or <see cref="Exception.HResult" /> is zero (0).
        /// </exception>
        protected ConnectionStringNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// The name of the connection string.
        /// </summary>
        public string ConnectionStringName { get; set; }
    }
}

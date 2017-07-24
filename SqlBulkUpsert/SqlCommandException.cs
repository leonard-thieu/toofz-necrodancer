using System;
using System.Text;

namespace SqlBulkUpsert
{
    public sealed class SqlCommandException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandException"/> class with a specified error message and
        /// a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic)
        /// if no inner exception is specified.
        /// </param>
        /// <param name="commandText">
        /// The Transact-SQL statement, table name or stored procedure that was attempted to be executed.
        /// </param>
        public SqlCommandException(string message, Exception inner, string commandText) : base(message, inner)
        {
            CommandText = commandText;
        }

        /// <summary>
        /// Gets or sets the Transact-SQL statement, table name or stored procedure that was attempted to be executed.
        /// </summary>
        public string CommandText { get; }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(base.ToString());
            sb.AppendLine();
            sb.AppendLine(CommandText);

            return sb.ToString();
        }
    }
}
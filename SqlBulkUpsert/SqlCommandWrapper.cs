using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SqlBulkUpsert
{
    /// <summary>
    /// Wraps an instance of <see cref="SqlCommand"/> to provide more detailed error information.
    /// </summary>
    internal sealed class SqlCommandWrapper : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandWrapper"/> class.
        /// </summary>
        /// <param name="command">The <see cref="SqlCommand"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="command"/> is null.
        /// </exception>
        public SqlCommandWrapper(SqlCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            this.command = command;
        }

        private readonly SqlCommand command;

        /// <summary>
        /// Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.
        /// </summary>
        public string CommandText
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(GetType().Name);

                return command.CommandText;
            }
            set
            {
                if (disposed)
                    throw new ObjectDisposedException(GetType().Name);

                command.CommandText = value;
            }
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery()
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new SqlCommandException(ex.Message, ex, CommandText);
            }
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (SqlException ex)
            {
                throw new SqlCommandException(ex.Message, ex, CommandText);
            }
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Releases all resources used by the <see cref="SqlCommandWrapper"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                command.Dispose();
            }

            disposed = true;
        }

        #endregion IDisposable Members
    }
}
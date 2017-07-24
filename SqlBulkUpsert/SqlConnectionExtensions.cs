using System;
using System.Data.SqlClient;

namespace SqlBulkUpsert
{
    /// <summary>
    /// Contains extension methods for <see cref="SqlConnection"/>.
    /// </summary>
    internal static class SqlConnectionExtensions
    {
        /// <summary>
        /// Creates and returns an instance of <see cref="SqlCommandWrapper"/> that wraps a <see cref="SqlCommand"/> that is
        /// associated with the <see cref="SqlConnection"/>.
        /// </summary>
        /// <param name="connection">The <see cref="SqlConnection"/> to create the command for.</param>
        /// <returns>An instance of <see cref="SqlCommandWrapper"/> that wraps a <see cref="SqlCommand"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection"/> is null.
        /// </exception>
        public static SqlCommandWrapper CreateWrappedCommand(this SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            return new SqlCommandWrapper(connection.CreateCommand());
        }
    }
}
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using static SqlBulkUpsert.Util;

namespace SqlBulkUpsert
{
    internal sealed class TemporaryTable : IDisposable
    {
        public static async Task<TemporaryTable> CreateAsync(SqlConnection connection, string targetTableName, CancellationToken cancellationToken)
        {
            var table = new TemporaryTable(connection, targetTableName);

            using (var command = connection.CreateWrappedCommand())
            {
                command.CommandText = Invariant("SELECT TOP 0 * INTO [{0}] FROM [{1}];", table.Name, targetTableName);
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }

            return table;
        }

        private TemporaryTable(SqlConnection connection, string targetTableName)
        {
            Name = Invariant("#{0}", targetTableName);

            this.connection = connection;
            this.targetTableName = targetTableName;
        }

        private readonly SqlConnection connection;
        private readonly string targetTableName;

        public string Name { get; }

        public Task<int> MergeAsync(SqlTableSchema targetTableSchema, bool updateOnMatch, CancellationToken cancellationToken, string sourceSearchCondition = null)
        {
            using (var command = connection.CreateWrappedCommand())
            {
                var mergeCommand = new MergeCommand(Name, targetTableSchema, updateOnMatch, sourceSearchCondition);
                command.CommandText = mergeCommand.ToString();

                return command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        #region IDisposable Members

        private bool disposed;

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
                // Drops non-temporary "temporary" tables
                if (Name != null && !Name.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                {
                    using (var command = connection.CreateWrappedCommand())
                    {
                        command.CommandText = Invariant("DROP TABLE [{0}];", Name);
                        command.ExecuteNonQuery();
                    }
                }
            }

            disposed = true;
        }

        #endregion
    }
}
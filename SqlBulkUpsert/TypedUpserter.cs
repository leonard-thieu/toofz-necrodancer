using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SqlBulkUpsert.Util;

namespace SqlBulkUpsert
{
    public sealed class TypedUpserter<T>
    {
        public TypedUpserter(SqlTableSchema targetTableSchema, ColumnMappings<T> columnMappings)
        {
            if (targetTableSchema == null)
                throw new ArgumentNullException(nameof(targetTableSchema));
            if (columnMappings == null)
                throw new ArgumentNullException(nameof(columnMappings));

            this.targetTableSchema = targetTableSchema;
            this.columnMappings = columnMappings;
        }

        private readonly SqlTableSchema targetTableSchema;
        private readonly ColumnMappings<T> columnMappings;

        public async Task InsertAsync(SqlConnection connection, IEnumerable<T> items, IProgress<long> progress)
        {
            var cancellationToken = CancellationToken.None;

            using (var command = connection.CreateWrappedCommand())
            {
                command.CommandText = Invariant("TRUNCATE TABLE {0};", targetTableSchema.TableName);
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }

            using (var dataReader = new TypedDataReader<T>(columnMappings, items))
            {
                progress.Report(items.Count());
                await BulkCopyAsync(connection, targetTableSchema.TableName, dataReader, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task UpsertAsync(SqlConnection connection, IEnumerable<T> items, IProgress<long> progress, bool updateOnMatch, CancellationToken cancellationToken)
        {
            using (var tempTable = await TemporaryTable.CreateAsync(connection, targetTableSchema.TableName, cancellationToken).ConfigureAwait(false))
            using (var dataReader = new TypedDataReader<T>(columnMappings, items))
            {
                await BulkCopyAsync(connection, tempTable.Name, dataReader, cancellationToken).ConfigureAwait(false);

                var rows = await tempTable.MergeAsync(targetTableSchema, updateOnMatch, cancellationToken).ConfigureAwait(false);
                progress.Report(rows);
            }
        }

        private async Task BulkCopyAsync(SqlConnection connection, string tableName, IDataReader data, CancellationToken cancellationToken)
        {
            using (var copy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, null))
            {
                foreach (var columnName in columnMappings.Keys)
                {
                    copy.ColumnMappings.Add(columnName, columnName);
                }

                copy.BulkCopyTimeout = 0;
                copy.DestinationTableName = tableName;

                await copy.WriteToServerAsync(data, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
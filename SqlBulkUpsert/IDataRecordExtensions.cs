using System;
using System.Data;

namespace SqlBulkUpsert
{
    /// <summary>
    /// Contains extension methods for <see cref="IDataRecord"/>.
    /// </summary>
    internal static class IDataRecordExtensions
    {
        public static T GetValue<T>(this IDataRecord reader, string columnName)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (columnName == null)
                throw new ArgumentNullException(nameof(columnName));

            var ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal))
                return default(T);
            return (T)reader.GetValue(ordinal);
        }
    }
}
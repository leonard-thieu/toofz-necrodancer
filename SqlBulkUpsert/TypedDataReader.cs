using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlBulkUpsert
{
    internal sealed class TypedDataReader<T> : IDataReader
    {
        public TypedDataReader(ColumnMappings<T> columnMappings, IEnumerable<T> items)
        {
            if (columnMappings == null)
                throw new ArgumentNullException(nameof(columnMappings));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            mappingLookup = columnMappings.Keys
                .Select((s, i) => new { Key = s, Value = i })
                .ToDictionary(x => x.Key, x => x.Value);
            mappingFuncs = columnMappings.Values.ToList();
            this.items = items.GetEnumerator();
        }

        private readonly Dictionary<string, int> mappingLookup;
        private readonly IList<Func<T, object>> mappingFuncs;
        private readonly IEnumerator<T> items;

        public void Dispose() { }

        public object GetValue(int i)
        {
            return mappingFuncs[i](items.Current);
        }

        public int GetOrdinal(string name)
        {
            return mappingLookup[name];
        }

        public int FieldCount
        {
            get { return mappingFuncs.Count; }
        }

        public bool Read()
        {
            return items.MoveNext();
        }

        // Not used by SqlBulkCopy (satisfying interface only)

        #region IDataReader Members

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        object IDataRecord.this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        object IDataRecord.this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsClosed
        {
            get { throw new NotImplementedException(); }
        }

        public int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
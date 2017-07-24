using System;
using System.Data;
using static SqlBulkUpsert.Util;

namespace SqlBulkUpsert
{
    public class Column : IEquatable<Column>
    {
        public static Column CreateFromReader(IDataReader sqlDataReader)
        {
            if (sqlDataReader == null)
                throw new ArgumentNullException(nameof(sqlDataReader));

            Column column;
            var dataType = (string)sqlDataReader["DATA_TYPE"];

            switch (dataType)
            {
                case "bigint":
                case "numeric":
                case "bit":
                case "smallint":
                case "decimal":
                case "smallmoney":
                case "int":
                case "tinyint":
                case "money":
                case "float":
                case "real":
                    column = new NumericColumn();
                    break;

                case "date":
                case "datetimeoffset":
                case "datetime2":
                case "smalldatetime":
                case "datetime":
                case "time":
                    column = new DateColumn();
                    break;

                case "char":
                case "varchar":
                case "text":
                case "nchar":
                case "nvarchar":
                case "ntext":
                case "binary":
                case "varbinary":
                case "image":
                    column = new TextColumn();
                    break;

                default:
                    column = new Column();
                    break;
            }

            column.Populate(sqlDataReader);

            return column;
        }

        public string Name { get; set; }
        public int OrdinalPosition { get; set; }
        public bool Nullable { get; set; }
        public string DataType { get; set; }
        public bool CanBeInserted { get; set; } = true;
        public bool CanBeUpdated { get; set; } = true;

        public virtual Column Clone()
        {
            return CopyTo(new Column());
        }

        public virtual Column CopyTo(Column column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            column.Name = Name;
            column.OrdinalPosition = OrdinalPosition;
            column.Nullable = Nullable;
            column.DataType = DataType;
            column.CanBeInserted = CanBeInserted;
            column.CanBeUpdated = CanBeUpdated;

            return column;
        }

        public virtual bool Equals(Column other)
        {
            return
                other != null &&
                Name == other.Name &&
                OrdinalPosition == other.OrdinalPosition &&
                Nullable == other.Nullable &&
                DataType == other.DataType;
        }

        public string ToSelectListString()
        {
            return Invariant("[{0}]", Name);
        }

        public virtual string ToColumnDefinitionString()
        {
            return Invariant("{0} {1} {2}NULL", ToSelectListString(), ToFullDataTypeString(), Nullable ? "" : "NOT ");
        }

        public virtual string ToFullDataTypeString()
        {
            return DataType;
        }

        protected virtual void Populate(IDataReader sqlDataReader)
        {
            if (sqlDataReader == null)
                throw new ArgumentNullException(nameof(sqlDataReader));

            Name = (string)sqlDataReader["COLUMN_NAME"];
            OrdinalPosition = (int)sqlDataReader["ORDINAL_POSITION"];
            Nullable = ((string)sqlDataReader["IS_NULLABLE"]) == "YES";
            DataType = (string)sqlDataReader["DATA_TYPE"];
        }
    }
}
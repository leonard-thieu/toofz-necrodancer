﻿using System.Data;
using static SqlBulkUpsert.Util;

namespace SqlBulkUpsert
{
    public sealed class NumericColumn : Column
    {
        public int? Precision { get; set; }
        public int? Radix { get; set; }
        public int? Scale { get; set; }

        public override Column Clone()
        {
            return CopyTo(new NumericColumn());
        }

        public override Column CopyTo(Column column)
        {
            var numericColumn = (NumericColumn)column;
            numericColumn.Precision = Precision;
            numericColumn.Radix = Radix;
            numericColumn.Scale = Scale;

            return base.CopyTo(numericColumn);
        }

        public override bool Equals(Column other)
        {
            var numericColumn = other as NumericColumn;
            if (numericColumn == null)
                return false;

            return
                 base.Equals(other) &&
                 Precision == numericColumn.Precision &&
                 Radix == numericColumn.Radix &&
                 Scale == numericColumn.Scale;
        }

        protected override void Populate(IDataReader sqlDataReader)
        {
            base.Populate(sqlDataReader);

            Precision = sqlDataReader.GetValue<byte?>("NUMERIC_PRECISION");
            Radix = sqlDataReader.GetValue<short?>("NUMERIC_PRECISION_RADIX");
            Scale = sqlDataReader.GetValue<int?>("NUMERIC_SCALE");
        }

        public override string ToFullDataTypeString()
        {
            switch (DataType)
            {
                case "numeric":
                case "decimal":
                    return Invariant("{0}({1}, {2})", DataType, Precision, Scale);

                case "float":
                case "real":
                    return Invariant("{0}({1})", DataType, Radix);
            }

            return base.ToFullDataTypeString();
        }
    }
}
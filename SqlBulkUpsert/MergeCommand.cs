using System;
using System.Linq;
using System.Text;
using static SqlBulkUpsert.Util;

namespace SqlBulkUpsert
{
    internal sealed class MergeCommand
    {
        public MergeCommand(string tableSource, SqlTableSchema targetTableSchema, bool updateOnMatch, string sourceSearchCondition)
        {
            if (tableSource == null)
                throw new ArgumentNullException(nameof(tableSource));
            if (targetTableSchema == null)
                throw new ArgumentNullException(nameof(targetTableSchema));

            this.targetTableSchema = targetTableSchema;
            this.tableSource = tableSource;
            this.updateOnMatch = updateOnMatch;
            this.sourceSearchCondition = sourceSearchCondition;
        }

        private readonly SqlTableSchema targetTableSchema;
        private readonly string tableSource;
        private readonly bool updateOnMatch;
        private readonly string sourceSearchCondition;

        public override string ToString()
        {
            var targetTable = targetTableSchema.TableName;
            var tableSource = this.tableSource;
            var mergeSearchCondition = GetMergeSearchCondition();
            var sourceSearchCondition = GetSearchCondition(this.sourceSearchCondition);
            var setClause = GetSetClause();
            var columnList = GetValuesList();
            var valuesList = GetValuesList();

            var sb = new StringBuilder();

            sb.AppendFormatLine("MERGE INTO [{0}] AS [Target]", targetTable);
            sb.AppendFormatLine("USING [{0}] AS [Source]", tableSource);
            sb.AppendFormatLine("    ON ({0})", mergeSearchCondition);

            if (updateOnMatch)
            {
                sb.AppendFormatLine("WHEN MATCHED");
                sb.AppendFormatLine("    THEN");
                sb.AppendFormatLine("        UPDATE");
                sb.AppendFormatLine("        SET {0}", setClause);
            }

            sb.AppendFormatLine("WHEN NOT MATCHED");
            sb.AppendFormatLine("    THEN");
            sb.AppendFormatLine("        INSERT ({0})", columnList);

            if (this.sourceSearchCondition == null)
            {
                sb.AppendFormatLine("        VALUES ({0});", valuesList);
            }
            else
            {
                sb.AppendFormatLine("        VALUES ({0})", valuesList);
                sb.AppendFormatLine("WHEN NOT MATCHED BY SOURCE {0}", sourceSearchCondition);
                sb.AppendFormatLine("    THEN");
                sb.AppendFormatLine("        DELETE;");
            }

            return sb.ToString();
        }

        private string GetSearchCondition(string searchCondition)
        {
            if (searchCondition == null)
            {
                return string.Empty;
            }
            else
            {
                return Invariant("AND {0}", searchCondition);
            }
        }

        private string GetMergeSearchCondition()
        {
            if (targetTableSchema.PrimaryKeyColumns == null)
                throw new ArgumentNullException(nameof(targetTableSchema.PrimaryKeyColumns));

            var columns = from c in targetTableSchema.PrimaryKeyColumns
                          select Invariant("[Target].{0} = [Source].{0}", c.ToSelectListString());

            return string.Join(" AND ", columns);
        }

        private string GetSetClause()
        {
            // Exclude primary key and identity columns
            var columns = from c in targetTableSchema.Columns
                          where c.CanBeUpdated
                          select Invariant("[Target].{0} = [Source].{0}", c.ToSelectListString());

            return string.Join(",\r\n            ", columns);
        }

        private string GetValuesList()
        {
            var columns = from c in targetTableSchema.Columns
                          where c.CanBeInserted
                          select c;

            return columns.ToSelectListString();
        }
    }
}
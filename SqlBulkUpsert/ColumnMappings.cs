using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlBulkUpsert
{
    public sealed class ColumnMappings<T> : Dictionary<string, Func<T, object>>
    {
        public void Add(Expression<Func<T, object>> map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            MemberExpression operand;

            var box = map.Body as UnaryExpression;
            if (box != null)
            {
                // Value types need to be unboxed
                operand = (MemberExpression)box.Operand;
            }
            else
            {
                operand = (MemberExpression)map.Body;
            }
            var name = operand.Member.Name;

            var func = map.Compile();

            Add(name, func);
        }
    }
}
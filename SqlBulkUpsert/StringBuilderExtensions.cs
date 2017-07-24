using System;
using System.Globalization;
using System.Text;

namespace SqlBulkUpsert
{
    /// <summary>
    /// Contains extension methods for <see cref="StringBuilder"/>.
    /// </summary>
    internal static class StringBuilderExtensions
    {
        public static void AppendFormatLine(this StringBuilder sb, string format, params object[] args)
        {
            if (sb == null)
                throw new ArgumentNullException(nameof(sb));
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            sb.AppendFormat(CultureInfo.InvariantCulture, format, args);
            sb.AppendLine();
        }
    }
}
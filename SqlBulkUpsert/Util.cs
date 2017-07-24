using System;
using System.Globalization;

namespace SqlBulkUpsert
{
    /// <summary>
    /// Contains utility methods.
    /// </summary>
    internal static class Util
    {
        /// <summary>
        /// Replaces the format items in a specified string with the string representations of corresponding objects
        /// in a specified array using the invariant culture.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string representation of
        /// the corresponding objects in args.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="format"/> or <paramref name="args"/> is null.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="format"/> is invalid. -or-
        /// The index of a format item is less than zero, or greater than or equal to the length of the args array.
        /// </exception>
        public static string Invariant(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
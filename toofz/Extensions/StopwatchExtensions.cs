using System.Globalization;

namespace System.Diagnostics
{
    /// <summary>
    /// Contains extension methods for <see cref="Stopwatch" />.
    /// </summary>
    public static class StopwatchExtensions
    {
        /// <summary>
        /// Gets the elapsed time expressed in whole and fractional seconds.
        /// </summary>
        /// <param name="stopwatch">The <see cref="Stopwatch" /> to get the elapsed time for.</param>
        /// <returns>The total number of seconds represented by this instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stopwatch" /> is null.
        /// </exception>
        public static double ToTotalSeconds(this Stopwatch stopwatch)
        {
            if (stopwatch == null)
                throw new ArgumentNullException(nameof(stopwatch));

            return stopwatch.Elapsed.TotalSeconds;
        }

        /// <summary>
        /// Gets the elapsed time expressed in whole and fractional seconds as a <see cref="string" />.
        /// </summary>
        /// <param name="stopwatch">The <see cref="Stopwatch" /> to get the elapsed time for.</param>
        /// <param name="format">A numeric format string.</param>
        /// <returns>The total number of seconds represented by this instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stopwatch" /> is null.
        /// </exception>
        /// <remarks>
        /// This method calls <see cref="double.ToString(string, IFormatProvider)" />.
        /// </remarks>
        public static string ToTotalSeconds(this Stopwatch stopwatch, string format)
        {
            if (stopwatch == null)
                throw new ArgumentNullException(nameof(stopwatch));

            return stopwatch.Elapsed.TotalSeconds.ToString(format, CultureInfo.CurrentCulture);
        }
    }
}

using System;
using System.IO;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Contains extension methods for <see cref="TextWriter"/>.
    /// </summary>
    static class TextWriterExtensions
    {
        /// <summary>
        /// Writes a line terminator, followed by the text representation of an object by calling the ToString method on 
        /// that object to the text string or stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="value">The value to write.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="writer"/> is null.
        /// </exception>
        public static void WriteLineStart(this TextWriter writer, object value)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer), $"{nameof(writer)} is null.");

            writer.WriteLine();
            writer.Write(value);
        }
    }
}

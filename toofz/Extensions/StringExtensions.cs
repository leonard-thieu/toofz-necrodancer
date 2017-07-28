﻿using System.Globalization;
using System.IO;
using System.Linq;

namespace System
{
    /// <summary>
    /// Contains extension methods for <see cref="string" />.
    /// </summary>
    public static class StringExtensions
    {
        #region ToTitleCase

        /// <summary>
        /// Converts the specified string to title case.
        /// </summary>
        /// <param name="value">The string to convert to title case.</param>
        /// <returns>
        /// The specified string converted to title case or null if <paramref name="value" /> is null.
        /// </returns>
        public static string ToTitleCase(this string value)
        {
            if (value == null)
                return null;

            var culture = CultureInfo.InvariantCulture;
            var ti = culture.TextInfo;

            var firstPass = ti.ToTitleCase(value.Replace('_', ' ').ToLower(culture));

            return AdjustForTitleCaseExceptions(firstPass).Replace("Hud", "HUD");
        }

        /// <summary>
        /// A collection of words that should not have the first letter capitalized. This list includes 
        /// articles, conjunctions, and prepositions less than six letters long.
        /// </summary>
        static readonly string[] TitleCaseExceptions = new[]
        {
            "abaft", "about", "above", "afore", "after", "along", "amid", "among", "an", "and", "apud", "as", "aside", "at",
            "atop", "below", "but", "by", "circa", "down", "for", "from", "given", "in", "into", "lest", "like", "mid", "midst",
            "minus", "near", "next", "nor", "of", "off", "on", "onto", "or", "out", "over", "pace", "past", "per", "plus", "pro",
            "qua", "round", "sans", "save", "since", "so", "than", "the", "thru", "till", "times", "to", "under", "until", "unto",
            "up", "upon", "via", "vice", "with", "worth", "yet"
        };

        static string AdjustForTitleCaseExceptions(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var tokens = value.Split(' ');
            for (int i = 1; i < tokens.Length; i++)
            {
                var match = TitleCaseExceptions.SingleOrDefault(e => tokens[i].Equals(e, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    tokens[i] = match;
                }
            }

            return string.Join(" ", tokens);
        }

        #endregion

        public static Stream ToStream(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(value);
            sw.Flush();
            ms.Position = 0;

            return ms;
        }
    }
}

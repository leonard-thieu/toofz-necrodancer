using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace System
{
    /// <summary>
    /// Contains extension methods for <see cref="string" />.
    /// </summary>
    public static class StringExtensions
    {
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
        private static readonly string[] TitleCaseExceptions = new[]
        {
            "abaft", "about", "above", "afore", "after", "along", "amid", "among", "an", "and", "apud", "as", "aside", "at",
            "atop", "below", "but", "by", "circa", "down", "for", "from", "given", "in", "into", "lest", "like", "mid", "midst",
            "minus", "near", "next", "nor", "of", "off", "on", "onto", "or", "out", "over", "pace", "past", "per", "plus", "pro",
            "qua", "round", "sans", "save", "since", "so", "than", "the", "thru", "till", "times", "to", "under", "until", "unto",
            "up", "upon", "via", "vice", "with", "worth", "yet"
        };

        private static string AdjustForTitleCaseExceptions(string value)
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

        public static IEnumerable<string> GetMostCommonSubstrings(this IEnumerable<string> strings)
        {
            if (strings == null)
                throw new ArgumentNullException(nameof(strings));
            if (!strings.Any())
                throw new ArgumentException();

            var allSubstrings = new List<List<string>>();

            foreach (var str in strings)
            {
                if (string.IsNullOrEmpty(str))
                    throw new ArgumentException();

                var substrings = new List<string>();

                for (int c = 0; c < str.Length - 1; c++)
                {
                    for (int cc = 1; c + cc <= str.Length; cc++)
                    {
                        var substr = str.Substring(c, cc);
                        if (allSubstrings.Count < 1 || allSubstrings.Last().Contains(substr))
                            substrings.Add(substr);
                    }
                }

                allSubstrings.Add(substrings);
            }

            if (allSubstrings.LastOrDefault().Any())
            {
                return from str in allSubstrings.LastOrDefault()
                       group str by str into g
                       orderby g.Key.Length descending, g.Count() descending
                       select g.Key;
            }

            return Enumerable.Empty<string>();
        }

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Deserializes Steam leaderboard related types.
    /// </summary>
    public sealed class LeaderboardsReader
    {
        #region Static Members

        static string CleanInvalidXmlChars(string text)
        {
            // From xml spec valid chars: 
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]     
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF. 
            var re = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-\u10FFFF]";

            return Regex.Replace(text, re, "");
        }

        #endregion

        /// <summary>
        /// Deserializes a <see cref="Replay"/> object from <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The serialized data that represents a <see cref="Replay"/> object.</param>
        /// <returns>A <see cref="Replay"/> object that represents the data.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is null.
        /// </exception>
        public Uri ReadReplayUri(Stream data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var uri = XDocument.Load(data).Element("data").Element("url").Value;

            return new Uri(uri);
        }
    }
}

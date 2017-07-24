using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using toofz.Xml;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Deserializes Steam leaderboard related types.
    /// </summary>
    public sealed class LeaderboardsReader : ILeaderboardsReader
    {
        #region Static Members

        private static readonly XmlSerializer LeaderboardSerializer = new XmlSerializer(typeof(Leaderboard));

        private static string CleanInvalidXmlChars(string text)
        {
            // From xml spec valid chars: 
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]     
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF. 
            var re = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-\u10FFFF]";

            return Regex.Replace(text, re, "");
        }

        #endregion

        /// <summary>
        /// Deserializes a list of <see cref="LeaderboardHeader"/>.
        /// </summary>
        /// <param name="data">The serialized data that represents a list of <see cref="LeaderboardHeader"/>.</param>
        /// <returns>A list of <see cref="LeaderboardHeader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is null.
        /// </exception>
        public IEnumerable<LeaderboardHeader> ReadLeaderboardHeaders(Stream data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            XElement root;
            using (var sr = new StreamReader(data))
            {
                var text = CleanInvalidXmlChars(sr.ReadToEnd());
                var xdoc = XDocument.Parse(text);
                root = xdoc.Element("response");
            }

            return (from l in root.Elements("leaderboard")
                    select new LeaderboardHeader
                    {
                        Address = l.Element("url").Value,
                        LeaderboardId = int.Parse(l.Element("lbid").Value),
                        Name = l.Element("name").Value,
                    }).ToList();
        }

        /// <summary>
        /// Deserializes a <see cref="Leaderboard"/> object from <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The serialized data that represents a <see cref="Leaderboard"/> object.</param>
        /// <returns>A <see cref="Leaderboard"/> object that represents the data.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is null.
        /// </exception>
        public Leaderboard ReadLeaderboard(Stream data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return LeaderboardSerializer.Load<Leaderboard>(data);
        }

        /// <summary>
        /// Deserializes a collection of players from the stream.
        /// </summary>
        /// <param name="data">The serialized data that represents a collection of players.</param>
        /// <returns>A collection of players that represents the data.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is null.
        /// </exception>
        public IEnumerable<Player> ReadPlayers(Stream data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var responseShape = new
            {
                response = new
                {
                    players = new Player[0]
                }
            };

            var sr = new StreamReader(data);
            var text = sr.ReadToEnd();
            text = CleanInvalidXmlChars(text);

            var response = JsonConvert.DeserializeAnonymousType(text, responseShape).response;

            return response.players;
        }

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

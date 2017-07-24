using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using log4net;
using toofz.Xml;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// A Crypt of the NecroDancer leaderboard entry.
    /// </summary>
    [XmlRoot(XmlName)]
    public sealed class Entry : IXmlSerializable
    {
        private const string XmlName = "entry";

        private static readonly ILog Log = LogManager.GetLogger(typeof(Entry));

        /// <summary>
        /// The ID of the leaderboard associated with the entry.
        /// </summary>
        public int LeaderboardId { get; set; }
        /// <summary>
        /// The leaderboard associated with the entry.
        /// </summary>
        public Leaderboard Leaderboard { get; set; }
        /// <summary>
        /// The rank of the entry relative to other entries on the leaderboard.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// The Steam ID of the player who submitted the entry.
        /// </summary>
        public long SteamId { get; set; }
        /// <summary>
        /// The player who submitted the entry.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The ID of the replay associated with the entry. This may be null if there was no replay submitted 
        /// with the entry (UGCID = -1).
        /// </summary>
        public long? ReplayId { get; set; }

        /// <summary>
        /// The score achieved by the entry.
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// The zone that the player was in when the run ended.
        /// </summary>
        public int Zone { get; set; }
        /// <summary>
        /// The level that the player was in when the run ended.
        /// </summary>
        public int Level { get; set; }

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() => null;

        /// <summary>
        /// Generates a <see cref="Entry"/> from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(XmlName);

            do
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case "steamid": SteamId = reader.ReadElementContentAsLong(); break;
                            case "score": Score = reader.ReadElementContentAsInt(); break;
                            case "rank": Rank = reader.ReadElementContentAsInt(); break;
                            case "ugcid":
                                var replayId = reader.ReadElementContentAsString();
                                try
                                {
                                    switch (replayId)
                                    {
                                        case "-1":
                                        case "18446744073709551615":
                                            ReplayId = null;
                                            break;
                                        default:
                                            ReplayId = long.Parse(replayId);
                                            break;
                                    }
                                }
                                catch (OverflowException ex)
                                {
                                    ReplayId = null;
                                    Log.Warn($"Couldn't parse ugcid: '{replayId}'. {ex}");
                                }
                                break;
                            case "details":
                                var details = reader.ReadElementContentAsString();
                                Zone = Convert.ToInt32(details.Substring(0, 2), 16);
                                Level = Convert.ToInt32(details.Substring(8, 2), 16);
                                break;

                            default:
                                Log.Warn($"Unknown element '{reader.LocalName}'.");
                                break;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        reader.FindAndReadEndElement();
                        return;

                    default:
                        if (!reader.Read())
                        {
                            return;
                        }
                        break;
                }
            } while (true);
        }

        /// <summary>
        /// Writing a <see cref="Entry"/> to its XML representation is not supported.
        /// </summary>
        /// <param name="writer">This parameter is not used.</param>
        /// <exception cref="NotSupportedException">
        /// Writing a <see cref="Entry"/> to its XML representation is not supported.
        /// </exception>
        public void WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException($"Writing a {nameof(Entry)} to its XML representation is not supported.");
        }

        #endregion
    }
}

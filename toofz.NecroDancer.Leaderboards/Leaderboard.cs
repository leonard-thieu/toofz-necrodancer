using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using log4net;
using toofz.Xml;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer leaderboard.
    /// </summary>
    [DebuggerDisplay("{Character} - {Run}")]
    [XmlRoot(XmlName)]
    public sealed class Leaderboard : IXmlSerializable
    {
        private const string XmlName = "response";

        private static readonly ILog Log = LogManager.GetLogger(typeof(Leaderboard));

        /// <summary>
        /// A value that uniquely identifies the leaderboard.
        /// </summary>
        public int LeaderboardId { get; set; }
        /// <summary>
        /// The total number of entries for the leaderboard. This value is provided by Steam.
        /// </summary>
        public int EntriesCount { get; set; }

        /// <summary>
        /// The leaderboard's collection of entries.
        /// </summary>
        public List<Entry> Entries { get; } = new List<Entry>();

        /// <summary>
        /// The last time that the leaderboard was updated.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// The ID of the character associated with the leaderboard.
        /// </summary>
        public int CharacterId { get; set; }
        /// <summary>
        /// The ID of the run associated with the leaderboard.
        /// </summary>
        public int RunId { get; set; }
        /// <summary>
        /// The date associated with the leaderboard if it is a daily. This value is null for leaderboards 
        /// that are not dailies.
        /// </summary>
        public DateTime? Date { get; set; }

        #region IXmlSerializable Members

        private static readonly XmlSerializer EntrySerializer = new XmlSerializer(typeof(Entry));

        /// <summary>
        /// This method is reserved and should not be used.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() => null;

        /// <summary>
        /// Generates a <see cref="Leaderboard"/> from its XML representation.
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
                            case "appID":
                            case "appFriendlyName":
                            case "entryStart":
                            case "entryEnd":
                            case "resultCount": reader.ReadElementContentAsString(); break;

                            case "leaderboardID": LeaderboardId = reader.ReadElementContentAsInt(); break;
                            case "totalLeaderboardEntries": EntriesCount = reader.ReadElementContentAsInt(); break;
                            case "nextRequestURL": reader.ReadElementContentAsString(); break;
                            case "entries":
                                reader.ReadNestedSequence<Entry, Entry>("entries", Entries, EntrySerializer, e => e.LeaderboardId = LeaderboardId);
                                break;

                            default:
                                Debugger.Break();
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
        /// Writing a <see cref="Leaderboard"/> to its XML representation is not supported.
        /// </summary>
        /// <param name="writer">This parameter is not used.</param>
        /// <exception cref="NotSupportedException">
        /// Writing a <see cref="Leaderboard"/> to its XML representation is not supported.
        /// </exception>
        public void WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException($"Writing a {nameof(Leaderboard)} to its XML representation is not supported.");
        }

        #endregion
    }
}

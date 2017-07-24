using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using toofz.Xml;

namespace toofz.NecroDancer.Data
{
    [XmlRoot(XmlName)]
    public sealed class Stats : IXmlSerializable
    {
        public const string XmlName = "stats";

        public int BeatsPerMove { get; set; }
        public int CoinsToDrop { get; set; }
        public int DamagePerHit { get; set; }
        public int Health { get; set; }
        public string Movement { get; set; }
        public int? Priority { get; set; }

        #region IXmlSerializable Members

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            var isEmpty = reader.IsEmptyElement;

            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "beatsPerMove": BeatsPerMove = reader.ReadContentAsInt(); break;
                    case "coinsToDrop": CoinsToDrop = reader.ReadContentAsInt(); break;
                    case "damagePerHit": DamagePerHit = reader.ReadContentAsInt(); break;
                    case "health": Health = reader.ReadContentAsInt(); break;
                    case "movement": Movement = reader.ReadContentAsString(); break;
                    case "priority": Priority = reader.ReadContentAsInt(); break;
                    default:
                        Trace.TraceWarning($"Unknown attribute '{reader.LocalName}'.");
                        break;
                }
            }

            reader.ReadStartElement(XmlName);

            if (isEmpty)
                return;

            reader.FindAndReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

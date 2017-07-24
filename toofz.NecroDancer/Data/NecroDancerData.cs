using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using toofz.Xml;

namespace toofz.NecroDancer.Data
{
    // Game version: 1.09
    [XmlRoot(XmlName)]
    public sealed class NecroDancerData : IXmlSerializable
    {
        #region Static Members

        public const string XmlName = "necrodancer";

        private static readonly XmlSerializer GameDataSerializer = new XmlSerializer(typeof(NecroDancerData));
        private static readonly XmlSerializer ItemSerializer = new XmlSerializer(typeof(Item));
        private static readonly XmlSerializer EnemySerializer = new XmlSerializer(typeof(Enemy));

        public static NecroDancerData Load(string path) => GameDataSerializer.Load<NecroDancerData>(path);

        #endregion

        #region Properties

        public List<Item> Items { get; } = new List<Item>();
        public List<Enemy> Enemies { get; } = new List<Enemy>();

        #endregion

        #region IXmlSerializable Members

        public XmlSchema GetSchema() => null;

        /// <summary>
        /// Reads game data from XML.
        /// </summary>
        /// <exception cref="XmlException">
        /// An error occurred while parsing the XML.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(XmlName);
            reader.ReadNestedSequence(Item.XmlArrayName, Items, ItemSerializer);
            reader.ReadNestedSequence(Enemy.XmlArrayName, Enemies, EnemySerializer);
            reader.FindAndReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

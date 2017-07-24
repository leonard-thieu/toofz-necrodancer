using System.Xml;
using System.Xml.Serialization;
using toofz.Xml;

namespace toofz.NecroDancer.Saves
{
    // Game version: 1.09
    [XmlRoot("necrodancer")]
    public sealed class SaveData
    {
        private static readonly XmlSerializer SaveDataSerializer = new XmlSerializer(typeof(SaveData));

        public static SaveData Load(string path)
        {
            return SaveDataSerializer.Load<SaveData>(path);
        }

        [XmlAttribute("cloudTimestamp")]
        public int CloudTimestamp { get; set; }

        [XmlElement("player")]
        public Player Player { get; set; }
        [XmlElement("game")]
        public Game Game { get; set; }
        [XmlElement("npc")]
        public Npc Npc { get; set; }
    }
}

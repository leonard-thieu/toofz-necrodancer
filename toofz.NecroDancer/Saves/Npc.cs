using System.Xml.Serialization;

namespace toofz.NecroDancer.Saves
{
    public sealed class Npc
    {
        [XmlAttribute("beastmaster")]
        public bool Beastmaster { get; set; }
        [XmlAttribute("beastmaster_visited")]
        public bool BeastmasterVisited { get; set; }
        [XmlAttribute("bossmaster")]
        public bool Bossmaster { get; set; }
        [XmlAttribute("bossmaster_visited")]
        public bool BossmasterVisited { get; set; }
        [XmlAttribute("hephaestus_visited")]
        public bool HephaestusVisited { get; set; }
        [XmlAttribute("janitor_visited")]
        public bool JanitorVisited { get; set; }
        [XmlAttribute("medic_visited")]
        public bool MedicVisited { get; set; }
        [XmlAttribute("merlin")]
        public bool Merlin { get; set; }
        [XmlAttribute("merlin_visited")]
        public bool MerlinVisited { get; set; }
    }
}

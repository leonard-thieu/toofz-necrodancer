using System.Xml.Serialization;

namespace toofz.NecroDancer.Data
{
    [XmlRoot("tweens")]
    public sealed class Tweens
    {
        [XmlAttribute("move")]
        public string Move { get; set; }
        [XmlAttribute("moveShadow")]
        public string MoveShadow { get; set; }
        [XmlAttribute("hit")]
        public string Hit { get; set; }
        [XmlAttribute("hitShadow")]
        public string HitShadow { get; set; }
    }
}

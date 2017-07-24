using System.ComponentModel;
using System.Xml.Serialization;

namespace toofz.NecroDancer.Data
{
    [XmlRoot("bouncer")]
    public class Bouncer
    {
        [XmlAttribute("min")]
        [DefaultValue(0)]
        public double Min { get; set; }
        [XmlAttribute("max")]
        [DefaultValue(0)]
        public double Max { get; set; }
        [XmlAttribute("power")]
        [DefaultValue(0)]
        public double Power { get; set; }
        [XmlAttribute("steps")]
        [DefaultValue(0)]
        public int Steps { get; set; }
    }
}

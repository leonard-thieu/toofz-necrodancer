using System.Xml.Serialization;

namespace toofz.NecroDancer.Data
{
    [XmlRoot("frame")]
    public sealed class Frame
    {
        [XmlAttribute("inSheet")]
        public int InSheet { get; set; }
        [XmlAttribute("inAnim")]
        public int InAnim { get; set; }
        [XmlAttribute("animType")]
        public string AnimType { get; set; }
        [XmlAttribute("onFraction")]
        public double OnFraction { get; set; }
        [XmlAttribute("offFraction")]
        public double OffFraction { get; set; }
        [XmlAttribute("singleFrame")]
        public string SingleFrame { get; set; }
    }
}

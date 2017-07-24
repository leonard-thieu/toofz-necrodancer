using System.ComponentModel;
using System.Xml.Serialization;

namespace toofz.NecroDancer.Data
{
    [XmlRoot("shadow")]
    public sealed class Shadow
    {
        [XmlText]
        public string Path { get; set; }
        [XmlAttribute("xOff")]
        [DefaultValue(0)]
        public int OffsetX { get; set; }
        [XmlAttribute("yOff")]
        [DefaultValue(0)]
        public int OffsetY { get; set; }
    }
}

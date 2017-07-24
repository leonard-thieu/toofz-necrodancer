using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace toofz.NecroDancer.Data
{
    [XmlRoot("spritesheet")]
    [DebuggerDisplay("{Path}")]
    public sealed class SpriteSheet
    {
        [XmlText]
        public string Path { get; set; }
        [XmlAttribute("numFrames")]
        [DefaultValue(0)]
        public int FrameCount { get; set; }
        [XmlAttribute("frameW")]
        [DefaultValue(0)]
        public int FrameWidth { get; set; }
        [XmlAttribute("frameH")]
        [DefaultValue(0)]
        public int FrameHeight { get; set; }
        [XmlAttribute("xOff")]
        [DefaultValue(0)]
        public int OffsetX { get; set; }
        [XmlAttribute("yOff")]
        [DefaultValue(0)]
        public int OffsetY { get; set; }
        [XmlAttribute("zOff")]
        [DefaultValue(0)]
        public int OffsetZ { get; set; }
        [XmlAttribute("heartXOff")]
        [DefaultValue(0)]
        public int HeartOffsetX { get; set; }
        [XmlAttribute("heartYOff")]
        [DefaultValue(0)]
        public int HeartOffsetY { get; set; }
    }
}

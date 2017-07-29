using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;

namespace toofz.NecroDancer.Dungeons
{
    public abstract class Entity : INotifyPropertyChanged
    {
        [XmlIgnore]
        public Point Position { get; set; }

        [XmlAttribute("x")]
        public virtual int X { get; set; }

        [XmlAttribute("y")]
        public virtual int Y { get; set; }

        public Entity Clone() => (Entity)MemberwiseClone();

#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
    }
}

using System.Diagnostics;
using System.Xml.Serialization;

namespace toofz.NecroDancer.Data
{
    [XmlRoot("particle")]
    [DebuggerDisplay("{HitPath}")]
    public sealed class Particle
    {
        [XmlAttribute("hit")]
        public string HitPath { get; set; }
    }
}

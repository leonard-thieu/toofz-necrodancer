using System.Diagnostics;

namespace toofz.NecroDancer.Data
{
    [DebuggerDisplay("{HitPath}")]
    public sealed class Particle
    {
        public string HitPath { get; set; }
    }
}

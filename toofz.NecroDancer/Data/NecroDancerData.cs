using System.Collections.Generic;

namespace toofz.NecroDancer.Data
{
    // Game version: 2.55
    /// <summary>
    /// Represents information parsed from necrodancer.xml.
    /// </summary>
    /// <see cref="NecroDancerDataSerializer"/>
    public sealed class NecroDancerData
    {
        public List<Item> Items { get; } = new List<Item>();
        public List<Enemy> Enemies { get; } = new List<Enemy>();
        public List<Character> Characters { get; } = new List<Character>();
        public List<Mode> Modes { get; } = new List<Mode>();
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace toofz.NecroDancer.Replays
{
    [DebuggerDisplay("Zone: {Zone}-{Level}")]
    public sealed class LevelData
    {
        public int Zone { get; set; }
        public int Level { get; set; }

        public int Seed { get; set; }
        public int Unknown0 { get; set; }
        public int Unknown1 { get; set; }
        public int TotalBeats { get; set; }

        public ICollection<Player> Players { get; } = new Collection<Player>();
        public ICollection<int> RandomMoves { get; } = new Collection<int>();
        public ICollection<int> ItemRolls { get; } = new Collection<int>();
    }
}

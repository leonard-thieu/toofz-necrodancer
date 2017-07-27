using System.Collections.Generic;
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

        public List<Player> Players { get; } = new List<Player>();
        public List<int> RandomMoves { get; } = new List<int>();
        public List<int> ItemRolls { get; } = new List<int>();
    }
}

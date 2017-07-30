using System.Collections.Generic;

namespace toofz.NecroDancer.Replays
{
    public sealed class Player
    {
        //  0 - Cadence
        //  1 - Melody
        //  2 - Aria
        //  3 - Dorian
        //  4 - Eli
        //  5 - Monk
        //  6 - Dove
        //  7 - Coda
        //  8 - Bolt
        //  9 - Bard
        // 10 - Nocturna
        // 11 - Diamond
        // 12 - Mary
        // 13 - Tempo
        public int Character { get; set; }
        public List<Move> Moves { get; } = new List<Move>();
        public List<int> WrongMoveBeats { get; } = new List<int>();
    }
}

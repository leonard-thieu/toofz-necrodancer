using System.Collections.Generic;

namespace toofz.NecroDancer.Replays
{
    public sealed class Player
    {
        public Character Character { get; set; }
        public List<Move> Moves { get; } = new List<Move>();
        public List<int> WrongMoveBeats { get; } = new List<int>();
    }
}

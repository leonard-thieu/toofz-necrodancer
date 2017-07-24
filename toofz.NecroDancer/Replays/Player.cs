using System.Collections.Generic;

namespace toofz.NecroDancer.Replays
{
    public sealed class Player
    {
        public Character Character { get; set; }
        public ICollection<Move> Moves { get; } = new List<Move>();
        public ICollection<int> WrongMoveBeats { get; } = new List<int>();
    }
}

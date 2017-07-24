using System.Diagnostics;

namespace toofz.NecroDancer.Replays
{
    [DebuggerDisplay("{Name}")]
    public sealed class Move
    {
        public int Beat { get; set; }
        public int Id { get; set; }

        public string Name
        {
            get
            {
                switch (Id)
                {
                    case 0: return "Up";
                    case 1: return "Right";
                    case 2: return "Down";
                    case 3: return "Left";
                    case 4: return "LeftRight";
                    case 5: return "UpDown";
                    case 6: return "UpLeft";
                    case 7: return "UpRight";
                    case 8: return "DownLeft";
                    case 9: return "DownRight";
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 25:
                    case 27:
                    case 28:
                    default: return "Unknown";
                }
            }
        }
    }
}

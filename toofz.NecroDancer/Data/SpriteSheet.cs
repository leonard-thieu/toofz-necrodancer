using System.Diagnostics;

namespace toofz.NecroDancer.Data
{
    [DebuggerDisplay("{Path}")]
    public sealed class SpriteSheet
    {
        public string Path { get; set; }
        public int FrameCount { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int OffsetZ { get; set; }
        public int HeartOffsetX { get; set; }
        public int HeartOffsetY { get; set; }
    }
}

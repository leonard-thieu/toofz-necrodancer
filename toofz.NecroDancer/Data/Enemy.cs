using System.Collections.Generic;
using System.Diagnostics;

namespace toofz.NecroDancer.Data
{
    [DebuggerDisplay("{Name}")]
    public sealed class Enemy
    {
        public string ElementName { get; set; }
        public int Type { get; set; }

        public int? Id { get; set; }
        public string FriendlyName { get; set; }
        public bool LevelEditor { get; set; } = true;

        public Bouncer Bouncer { get; set; } = new Bouncer();
        public List<Frame> Frames { get; } = new List<Frame>();
        public OptionalStats OptionalStats { get; set; } = new OptionalStats();
        public Particle Particle { get; set; } = new Particle();
        public Shadow Shadow { get; set; } = new Shadow();
        public SpriteSheet SpriteSheet { get; set; } = new SpriteSheet();
        public Stats Stats { get; set; } = new Stats();
        public Tweens Tweens { get; set; } = new Tweens();

        public string Name { get; set; }
        public string ImagePath => SpriteSheet.Path;
        public int FrameCount
        {
            get { return SpriteSheet.FrameCount / 2; }
            set { SpriteSheet.FrameCount = value; }
        }
    }
}

using System.Xml.Serialization;
using FluentValidation;

namespace toofz.NecroDancer.Dungeons
{
    [XmlRoot("tile")]
    public sealed class Tile : Entity<Tile>
    {
        //   0 - false
        //   1 - true
        [XmlAttribute("cracked")]
        public int IsCracked { get; set; }
        //   0 - false
        //   1 - true
        [XmlAttribute("torch")]
        public int HasTorch { get; set; }
        //   0 - Floor
        //   1 - Floor
        //   2 - Stairs
        //   3 - Shop Floor
        //   4 - Water
        //   8 - Tar
        //   9 - Locked Stairs
        //  10 - Hot Coals
        //  11 - Ice
        //  14 - Boss Floor
        //  17 - Ooze
        // 100 - Dirt Wall
        // 101 - Dirt Wall
        // 103 - Door
        // 104 - Shop Wall
        // 105 - Unbreakable Wall
        // 106 - Locked Door
        // 107 - Stone Wall
        // 108 - Catacomb Wall
        // 109 - Boss Wall
        // 111 - Metal Door
        [XmlAttribute("type")]
        public int Type { get; set; }
        //   0 - Zone 1
        //   1 - Zone 2
        //   2 - Zone 3 Hot
        //   3 - Zone 3 Cold
        //   4 - Zone 4
        //   5 - Boss
        [XmlAttribute("zone")]
        public int Zone { get; set; }

        [XmlIgnore]
        public bool IsFloor => Type < 100;

        [XmlIgnore]
        public bool IsWall => Type >= 100;

        [XmlIgnore]
        public bool IsCrackable
        {
            get
            {
                switch (Type)
                {
                    case 100:
                    case 101:
                    case 104:
                    case 107:
                    case 108: return true;
                    default: return false;
                }
            }
        }

        [XmlIgnore]
        public bool IsTorchable => true;
    }

    sealed class TileValidator : AbstractValidator<Tile>
    {
        public TileValidator()
        {
            RuleFor(x => x.IsCracked).IsIntegralBoolean();
            RuleFor(x => x.HasTorch).IsIntegralBoolean();
            RuleFor(x => x.Zone).InclusiveBetween(0, 5);
        }
    }
}

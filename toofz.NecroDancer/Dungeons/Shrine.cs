using System.Xml.Serialization;
using FluentValidation;

namespace toofz.NecroDancer.Dungeons
{
    [XmlRoot("shrine")]
    public sealed class Shrine : Entity<Shrine>
    {
        //  0 - Blood
        //  1 - Darkness
        //  2 - Glass
        //  3 - Peace
        //  4 - Rhythm
        //  5 - Risk
        //  6 - Sacrifice
        //  7 - Space
        //  8 - War
        //  9 - No Return
        // 10 - Phasing
        // 11 - Pace
        // 12 - Chance
        [XmlAttribute("type")]
        public int Type { get; set; }
    }

    sealed class ShrineValidator : AbstractValidator<Shrine>
    {
        public ShrineValidator()
        {
            RuleFor(x => x.Type).InclusiveBetween(0, 12);
        }
    }
}

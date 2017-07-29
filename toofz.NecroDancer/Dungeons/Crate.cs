using System.Xml.Serialization;
using FluentValidation;

namespace toofz.NecroDancer.Dungeons
{
    [XmlRoot("crate")]
    public sealed class Crate : Entity<Crate>
    {
        // "no_item" or Item.Type
        [XmlAttribute("contents")]
        public string Contents { get; set; } = Item.NoItem;
        // 0 - Crate
        // 1 - Barrel
        // 2 - Urn
        [XmlAttribute("type")]
        public int Type { get; set; }
    }

    sealed class CrateValidator : AbstractValidator<Crate>
    {
        public CrateValidator()
        {
            RuleFor(x => x.Type).InclusiveBetween(0, 2);
        }
    }
}

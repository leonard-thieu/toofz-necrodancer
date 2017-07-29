using System.Xml.Serialization;
using FluentValidation;

namespace toofz.NecroDancer.Dungeons
{
    [XmlRoot("chest")]
    public sealed class Chest : Entity<Chest>
    {
        // 1 - Red
        // 2 - Black
        // 3 - Purple
        [XmlAttribute("color")]
        public int Color { get; set; }
        // "no_item" or Item.Type
        [XmlAttribute("contents")]
        public string Contents { get; set; } = Item.NoItem;
        // 0 - false
        // 1 - true
        [XmlAttribute("hidden")]
        public int Hidden { get; set; }

        [XmlAttribute("saleCost")]
        public int SaleCost { get; set; }
        // 0 - false
        // 1 - true
        [XmlAttribute("singleChoice")]
        public int SingleChoice { get; set; }
    }

    sealed class ChestValidator : AbstractValidator<Chest>
    {
        public ChestValidator()
        {
            RuleFor(x => x.Color).InclusiveBetween(1, 3);
            RuleFor(x => x.Hidden).IsIntegralBoolean();
            RuleFor(x => x.SingleChoice).InclusiveBetween(0, 1);
        }
    }
}

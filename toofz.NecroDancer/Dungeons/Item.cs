using System.Xml.Serialization;
using FluentValidation;

namespace toofz.NecroDancer.Dungeons
{
    [XmlRoot("item")]
    public sealed class Item : Entity<Item>
    {
        public const string NoItem = "no_item";

        [XmlAttribute("bloodCost")]
        public double BloodCost { get; set; }
        [XmlAttribute("saleCost")]
        public int SaleCost { get; set; }
        // 0 - false
        // 1 - true
        [XmlAttribute("singleChoice")]
        public int SingleChoice { get; set; }
        [XmlAttribute("type")]
        public string Type { get; set; }
    }

    sealed class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {
            RuleFor(x => x.SingleChoice).IsIntegralBoolean();
        }
    }
}

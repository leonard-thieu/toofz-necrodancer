using System.Xml.Serialization;
using FluentValidation;

namespace toofz.NecroDancer.Dungeons
{
    [XmlRoot("enemy")]
    public sealed class Enemy : Entity<Enemy>
    {
        [XmlAttribute("beatDelay")]
        public int BeatDelay { get; set; }
        // 0 - false
        // 1 - true
        [XmlAttribute("lord")]
        public int Lord { get; set; }
        [XmlAttribute("type")]
        public int Id { get; set; }
        // -1 - None
        // 12 - King Conga 1
        // 13 - King Conga 2
        // 14 - King Conga 3
        // 15 - King Conga 4
        // 16 - Death Metal 1
        // 17 - Death Metal 2
        // 18 - Death Metal 3
        // 19 - Death Metal 4
        // 20 - Deep Blues 1
        // 21 - Deep Blues 2
        // 22 - Deep Blues 3
        // 23 - Deep Blues 4
        // 24 - Coral Riff 1
        // 25 - Coral Riff 2
        // 26 - Coral Riff 3
        // 27 - Coral Riff 4
        [XmlIgnore]
        public int Boss { get; set; }
    }

    sealed class EnemyValidator : AbstractValidator<Enemy>
    {
        public EnemyValidator()
        {
            RuleFor(x => x.Lord).SetValidator(new IntegralBooleanValidator());
            RuleFor(x => x.Boss).InclusiveBetween(12, 27).Unless(x => x.Boss == -1);
        }
    }
}

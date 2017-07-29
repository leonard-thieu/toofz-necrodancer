using System.Xml.Serialization;
using FluentValidation;

namespace toofz.NecroDancer.Dungeons
{
    [XmlRoot("trap")]
    public sealed class Trap : Entity<Trap>
    {
        // 1 - Bounce Trap
        // 2 - Spike Trap
        // 3 - Trap door
        // 4 - Confuse Trap
        // 5 - Teleport Trap
        // 6 - Tempo Down Trap
        // 7 - Tempo Up Trap
        // 8 - Teleport Rune
        // 9 - Bomb Trap
        [XmlAttribute("type")]
        public int Type { get; set; }
        // General
        //     -1 - None
        // Bounce Trap
        //      0 - Right
        //      1 - Left
        //      2 - Down
        //      3 - Up
        //      4 - Down Right
        //      5 - Down Left
        //      6 - Up Left
        //      7 - Up Right
        //      8 - Omni
        //      9 - Random
        // Teleport Rune
        //      1 - Transmogrification
        //      2 - Arena
        //      3 - Blood Shop
        //      4 - Glass Shop
        //      5 - Health Shop
        //      6 - Shriner
        //      7 - Pawnbroker
        //      8 - Summoner
        [XmlAttribute("subtype")]
        public int Subtype { get; set; }
    }

    sealed class TrapValidator : AbstractValidator<Trap>
    {
        public TrapValidator()
        {
            RuleFor(x => x.Type).InclusiveBetween(1, 9);
            RuleFor(x => x.Subtype).Must((x, s) =>
            {
                switch (x.Type)
                {
                    case 1:
                        switch (s)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9: return true;
                            default: return false;
                        }

                    case 8:
                        switch (s)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8: return true;
                            default: return false;
                        }

                    default: return s == -1;
                }
            });
        }
    }
}

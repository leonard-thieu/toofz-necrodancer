using System.Collections.Generic;
using System.ComponentModel;

namespace toofz.NecroDancer
{
    public static class Runs
    {
        public static readonly Run
        //
            Score = new Run(0, "Score", "Score"),
            Speed = new Run(1, "Speed", "Speed"),
            SeededScore = new Run(2, "SeededScore", "Seeded Score"),
            SeededSpeed = new Run(3, "SeededSpeed", "Seeded Speed"),
            Deathless = new Run(4, "Deathless", "Deathless"),
            Daily = new Run(5, "Daily", "Daily")
        //
        ;

        public static readonly IEnumerable<Run>
        //
            All = Enumeration.GetAll<Run>()
        //
        ;
    }

    [EnumerationContainer(typeof(Runs))]
    [TypeConverter(typeof(EnumerationTypeConverter<Run>))]
    public sealed class Run : Enumeration
    {
        private Run() : base(0, null) { }

        internal Run(int id, string name, string displayName) : base(id, name)
        {
            DisplayName = displayName;
        }

        public string DisplayName { get; private set; }
        public string Category
        {
            get
            {
                switch (Name)
                {
                    case "Score":
                    case "SeededScore":
                    case "Daily": return "Score";
                    case "Speed":
                    case "SeededSpeed": return "Speed";
                    case "Deathless": return "Deathless";
                    default: return "Unknown";
                }
            }
        }
        public string ScoreUnits
        {
            get
            {
                switch (Name)
                {
                    case "Score":
                    case "SeededScore":
                    case "Daily": return "Coins";
                    case "Speed":
                    case "SeededSpeed": return "Time";
                    case "Deathless": return "Wins";
                    default: return "Score";
                }
            }
        }
    }
}

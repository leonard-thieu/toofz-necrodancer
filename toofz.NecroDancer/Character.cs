using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace toofz.NecroDancer
{
    public static class Characters
    {
        public static readonly Character
        //
            AllCharactersAmplified = new Character(-4, "All Characters (Amplified)"),
            AllCharacters = new Character(-3, "All Characters"),
            Any = new Character(-1, "Any"),
            Aria = new Character(2, "Aria"),
            Bard = new Character(9, "Bard"),
            Bolt = new Character(8, "Bolt"),
            Cadence = new Character(0, "Cadence"),
            Coda = new Character(7, "Coda"),
            Diamond = new Character(11, "Diamond"),
            Dorian = new Character(3, "Dorian"),
            Dove = new Character(6, "Dove"),
            Eli = new Character(4, "Eli"),
            Mary = new Character(12, "Mary"),
            Melody = new Character(1, "Melody"),
            Monk = new Character(5, "Monk"),
            Nocturna = new Character(10, "Nocturna"),
            StoryMode = new Character(-2, "Story Mode"),
            Tempo = new Character(13, "Tempo")
        //
        ;

        public static readonly IEnumerable<Character>
        //
            All = Enumeration.GetAll<Character>(),
            Primary = new ReadOnlyCollection<Character>(new[] { AllCharacters, Aria, Bard, Bolt, Cadence, Coda, Dorian, Dove, Eli, Melody, Monk, StoryMode }),
            Deathless = new ReadOnlyCollection<Character>(new[] { AllCharacters, Aria, Bard, Bolt, Cadence, Coda, Dorian, Dove, Eli, Melody, Monk }),
            Daily = new ReadOnlyCollection<Character>(new[] { Cadence })
        //
        ;
    }

    [EnumerationContainer(typeof(Characters))]
    [TypeConverter(typeof(EnumerationTypeConverter<Character>))]
    public sealed class Character : Enumeration
    {
        private Character() : base(0, null) { }

        internal Character(int id, string name) : base(id, name) { }
    }
}

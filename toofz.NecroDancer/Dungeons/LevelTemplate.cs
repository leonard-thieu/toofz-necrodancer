using System.Collections.Generic;
using System.Linq;
using toofz.NecroDancer.Properties;

namespace toofz.NecroDancer.Dungeons
{
    public static class LevelTemplates
    {
        public static readonly LevelTemplate
        //
            Default = new LevelTemplate.Impl(-1, "None", Resources.Default),
            Conga1 = new LevelTemplate.Impl(12, "King Conga 1", Resources.KingConga1),
            Conga2 = new LevelTemplate.Impl(13, "King Conga 2", Resources.KingConga2),
            Conga3 = new LevelTemplate.Impl(14, "King Conga 3", Resources.KingConga3),
            Conga4 = new LevelTemplate.Impl(15, "King Conga 4", Resources.KingConga4),
            DeathMetal1 = new LevelTemplate.Impl(16, "Death Metal 1", Resources.DeathMetal1),
            DeathMetal2 = new LevelTemplate.Impl(17, "Death Metal 2", Resources.DeathMetal2),
            DeathMetal3 = new LevelTemplate.Impl(18, "Death Metal 3", Resources.DeathMetal3),
            DeathMetal4 = new LevelTemplate.Impl(19, "Death Metal 4", Resources.DeathMetal4),
            DeepBlues1 = new LevelTemplate.Impl(20, "Deep Blues 1", Resources.DeepBlues1),
            DeepBlues2 = new LevelTemplate.Impl(21, "Deep Blues 2", Resources.DeepBlues2),
            DeepBlues3 = new LevelTemplate.Impl(22, "Deep Blues 3", Resources.DeepBlues3),
            DeepBlues4 = new LevelTemplate.Impl(23, "Deep Blues 4", Resources.DeepBlues4),
            CoralRiff1 = new LevelTemplate.Impl(24, "Coral Riff 1", Resources.CoralRiff1),
            CoralRiff2 = new LevelTemplate.Impl(25, "Coral Riff 2", Resources.CoralRiff2),
            CoralRiff3 = new LevelTemplate.Impl(26, "Coral Riff 3", Resources.CoralRiff3),
            CoralRiff4 = new LevelTemplate.Impl(27, "Coral Riff 4", Resources.CoralRiff4)
        //
        ;

        public static readonly IEnumerable<LevelTemplate> Bosses = Enumeration.GetAll<LevelTemplate>().Where(t => t != Default);
    }

    [EnumerationContainer(typeof(LevelTemplates))]
    public abstract class LevelTemplate : Enumeration
    {
        LevelTemplate(int id, string name, string template) : base(id, name)
        {
            Template = template;
        }

        public string Template { get; private set; }

        internal class Impl : LevelTemplate
        {
            public Impl(int id, string name, string template) : base(id, name, template) { }
        }
    }
}

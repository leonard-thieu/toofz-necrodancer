using System.Data.Entity.Migrations;
using toofz.NecroDancer.Data;

namespace toofz.NecroDancer.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<NecroDancerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NecroDancerContext context)
        {
            var path = GetNecroDancerPath();
            var data = NecroDancerDataSerializer.Read(path);

            context.Set<Item>().AddOrUpdate(i => i.ElementName, data.Items.ToArray());
            context.Set<Enemy>().AddOrUpdate(e => new { e.ElementName, e.Type }, data.Enemies.ToArray());

            context.SaveChanges();
        }

        // TODO: This shouldn't be hardcoded.
        private static string GetNecroDancerPath()
        {
#if DEBUG
            return @"S:\Applications\Steam\steamapps\common\Crypt of the NecroDancer\data\necrodancer.xml";
#else
            return @"C:\GOG Games\Crypt of the Necrodancer\data\necrodancer.xml";
#endif
        }
    }
}

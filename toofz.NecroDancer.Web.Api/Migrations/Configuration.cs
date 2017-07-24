using System.Data.Entity.Migrations;
using toofz.NecroDancer.Web.Api.Identity;

namespace toofz.NecroDancer.Web.Api.Migrations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}

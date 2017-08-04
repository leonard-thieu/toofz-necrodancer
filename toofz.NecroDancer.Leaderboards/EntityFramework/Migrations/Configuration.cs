using System;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    [ExcludeFromCodeCoverage]
    sealed class Configuration : DbMigrationsConfiguration<LeaderboardsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Toofz.NecroDancer.Leaderboards.DataAccess.Migrations.Configuration";
            MigrationsDirectory = @"EntityFramework\Migrations";
        }

        protected override void Seed(LeaderboardsContext context)
        {
            base.Seed(context);
            
            var dailyLeaderboardHeaders = LeaderboardsResources.ReadDailyLeaderboardHeaders("daily-leaderboard-headers.min.json");
            var existing = (from l in context.DailyLeaderboards
                            select l.LeaderboardId)
                            .ToList();
            var newDailies = from h in dailyLeaderboardHeaders
                             where !existing.Contains(h.id)
                             select h;
            var categories = LeaderboardsResources.ReadCategories("leaderboard-categories.min.json");

            foreach (var newDaily in newDailies)
            {
                var leaderboard = new DailyLeaderboard
                {
                    LeaderboardId = newDaily.id,
                    LastUpdate = new DateTime(2000, 1, 1),
                    Date = newDaily.date,
                    IsProduction = newDaily.production,
                };
                leaderboard.ProductId = categories.GetItemId("products", newDaily.product);
                context.DailyLeaderboards.AddOrUpdate(leaderboard);
            }

            context.SaveChanges();
        }
    }
}

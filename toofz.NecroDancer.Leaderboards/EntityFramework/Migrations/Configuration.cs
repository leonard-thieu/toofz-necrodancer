using System;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Properties;
using System.Linq;

namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<LeaderboardsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Toofz.NecroDancer.Leaderboards.DataAccess.Migrations.Configuration";
        }

        protected override void Seed(LeaderboardsContext context)
        {
            base.Seed(context);

            DailyHeaders daily_headers;
            using (var r = new StringReader(Resources.DailyLeaderboardHeaders))
            {
                var serializer = new JsonSerializer();
                daily_headers = ((DailyHeaders)serializer.Deserialize(r, typeof(DailyHeaders)));
            }

            var existing = context.DailyLeaderboards.Select(l => l.LeaderboardId).ToList();
            var headers = daily_headers.leaderboards.Where(h => !existing.Contains(h.id));
            foreach (var header in headers)
            {
                var leaderboard = new DailyLeaderboard
                {
                    LeaderboardId = header.id,
                    LastUpdate = new DateTime(2000, 1, 1),
                    Date = header.date,
                    IsProduction = header.production,
                };
                switch (header.product)
                {
                    case "Classic": leaderboard.ProductId = 0; break;
                    case "Amplified": leaderboard.ProductId = 1; break;
                    default:
                        throw new Exception($"Unknown product: '{header.product}'");
                }
                context.DailyLeaderboards.AddOrUpdate(leaderboard);
            }

            context.SaveChanges();
        }
    }
}

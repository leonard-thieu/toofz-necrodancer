namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEntryFKFromReplay : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Replays", new[] { "SteamId", "LeaderboardId" }, "dbo.Entries");
            DropIndex("dbo.Replays", new[] { "SteamId", "LeaderboardId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Replays", new[] { "SteamId", "LeaderboardId" });
            AddForeignKey("dbo.Replays", new[] { "SteamId", "LeaderboardId" }, "dbo.Entries", new[] { "SteamId", "LeaderboardId" }, cascadeDelete: true);
        }
    }
}

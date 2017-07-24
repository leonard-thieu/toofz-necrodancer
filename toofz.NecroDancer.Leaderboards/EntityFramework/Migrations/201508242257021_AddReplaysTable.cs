namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReplaysTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Replays",
                c => new
                    {
                        ReplayId = c.Long(nullable: false),
                        SteamId = c.Long(nullable: false),
                        LeaderboardId = c.Int(nullable: false),
                        FileName = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.ReplayId)
                .ForeignKey("dbo.Entries", t => new { t.SteamId, t.LeaderboardId }, cascadeDelete: true)
                .Index(t => new { t.SteamId, t.LeaderboardId });
            
            AddColumn("dbo.Entries", "UgcId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Replays", new[] { "SteamId", "LeaderboardId" }, "dbo.Entries");
            DropIndex("dbo.Replays", new[] { "SteamId", "LeaderboardId" });
            DropColumn("dbo.Entries", "UgcId");
            DropTable("dbo.Replays");
        }
    }
}

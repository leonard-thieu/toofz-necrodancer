namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergedLeaderboardMetadataToLeaderboard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leaderboards", "DisplayName", c => c.String());
            DropColumn("dbo.Leaderboards", "Metadata_LeaderboardId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Leaderboards", "Metadata_LeaderboardId", c => c.Int(nullable: false));
            DropColumn("dbo.Leaderboards", "DisplayName");
        }
    }
}

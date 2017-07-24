namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLeaderboardIdColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leaderboards", "Metadata_LeaderboardId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leaderboards", "Metadata_LeaderboardId");
        }
    }
}

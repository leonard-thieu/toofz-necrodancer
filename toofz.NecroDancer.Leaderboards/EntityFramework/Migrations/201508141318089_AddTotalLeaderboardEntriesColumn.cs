namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalLeaderboardEntriesColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leaderboards", "TotalLeaderboardEntries", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leaderboards", "TotalLeaderboardEntries");
        }
    }
}

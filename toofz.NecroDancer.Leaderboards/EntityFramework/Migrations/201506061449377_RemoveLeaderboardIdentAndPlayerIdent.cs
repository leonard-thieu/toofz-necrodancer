namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLeaderboardIdentAndPlayerIdent : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Leaderboards", "LeaderboardIdent");
            DropColumn("dbo.Players", "PlayerIdent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "PlayerIdent", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Leaderboards", "LeaderboardIdent", c => c.Int(nullable: false, identity: true));
        }
    }
}

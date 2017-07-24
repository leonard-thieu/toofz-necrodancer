namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerLastUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "LastUpdate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "LastUpdate");
        }
    }
}

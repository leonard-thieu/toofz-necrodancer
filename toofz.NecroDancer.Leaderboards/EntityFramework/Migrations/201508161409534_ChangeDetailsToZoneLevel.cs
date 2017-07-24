namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDetailsToZoneLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "Zone", c => c.Int(nullable: false));
            AddColumn("dbo.Entries", "Level", c => c.Int(nullable: false));
            DropColumn("dbo.Entries", "Details");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Entries", "Details", c => c.String());
            DropColumn("dbo.Entries", "Level");
            DropColumn("dbo.Entries", "Zone");
        }
    }
}

namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVersionColumn1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Replays", "Version", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Replays", "Version");
        }
    }
}

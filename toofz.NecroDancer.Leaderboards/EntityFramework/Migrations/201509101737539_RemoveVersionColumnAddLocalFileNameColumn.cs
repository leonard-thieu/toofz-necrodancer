namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveVersionColumnAddLocalFileNameColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Replays", "LocalFileName", c => c.String());
            DropColumn("dbo.Replays", "Version");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Replays", "Version", c => c.Int(nullable: false));
            DropColumn("dbo.Replays", "LocalFileName");
        }
    }
}

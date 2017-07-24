namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDataValidColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Replays", "IsDataValid", c => c.Boolean(nullable: false));
            DropColumn("dbo.Replays", "FileName");
            DropColumn("dbo.Replays", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Replays", "Address", c => c.String());
            AddColumn("dbo.Replays", "FileName", c => c.String());
            DropColumn("dbo.Replays", "IsDataValid");
        }
    }
}

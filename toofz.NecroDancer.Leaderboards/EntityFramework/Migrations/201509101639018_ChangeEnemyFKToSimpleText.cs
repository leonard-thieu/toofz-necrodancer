namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEnemyFKToSimpleText : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Replays", "KilledBy", c => c.String());
            DropColumn("dbo.Replays", "ElementName");
            DropColumn("dbo.Replays", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Replays", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Replays", "ElementName", c => c.String());
            DropColumn("dbo.Replays", "KilledBy");
        }
    }
}

namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEnemyForeignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Replays", "ElementName", c => c.String());
            AddColumn("dbo.Replays", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Replays", "KilledBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Replays", "KilledBy", c => c.String());
            DropColumn("dbo.Replays", "Type");
            DropColumn("dbo.Replays", "ElementName");
        }
    }
}

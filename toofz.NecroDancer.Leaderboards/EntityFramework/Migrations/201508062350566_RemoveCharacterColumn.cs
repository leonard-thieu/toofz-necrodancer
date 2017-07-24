namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCharacterColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Leaderboards", "Character");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Leaderboards", "Character", c => c.Int(nullable: false));
        }
    }
}

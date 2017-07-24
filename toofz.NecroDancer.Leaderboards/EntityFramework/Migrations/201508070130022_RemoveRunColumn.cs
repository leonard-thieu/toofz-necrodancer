namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRunColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Leaderboards", "Run");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Leaderboards", "Run", c => c.Int(nullable: false));
        }
    }
}

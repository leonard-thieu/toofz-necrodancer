namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreasePlayerNameLength : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Players", new[] { "PersonaName" });
            AlterColumn("dbo.Players", "PersonaName", c => c.String(maxLength: 64));
            CreateIndex("dbo.Players", "PersonaName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Players", new[] { "PersonaName" });
            AlterColumn("dbo.Players", "PersonaName", c => c.String(maxLength: 32));
            CreateIndex("dbo.Players", "PersonaName");
        }
    }
}

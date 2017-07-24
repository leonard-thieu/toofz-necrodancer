namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToPlayerPersonaName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Players", "PersonaName", c => c.String(maxLength: 450));
            CreateIndex("dbo.Players", "PersonaName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Players", new[] { "PersonaName" });
            AlterColumn("dbo.Players", "PersonaName", c => c.String());
        }
    }
}

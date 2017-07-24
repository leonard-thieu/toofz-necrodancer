namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedUnusedColumnsAndAddedConstraints : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Players", new[] { "PersonaName" });
            AlterColumn("dbo.Players", "PersonaName", c => c.String(maxLength: 32));
            AlterColumn("dbo.Players", "CountryCode", c => c.String(maxLength: 2));
            AlterColumn("dbo.Players", "StateCode", c => c.String(maxLength: 3));
            CreateIndex("dbo.Players", "PersonaName");
            DropColumn("dbo.Leaderboards", "DisplayName");
            DropColumn("dbo.Players", "ProfileAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "ProfileAddress", c => c.String());
            AddColumn("dbo.Leaderboards", "DisplayName", c => c.String());
            DropIndex("dbo.Players", new[] { "PersonaName" });
            AlterColumn("dbo.Players", "StateCode", c => c.String());
            AlterColumn("dbo.Players", "CountryCode", c => c.String());
            AlterColumn("dbo.Players", "PersonaName", c => c.String(maxLength: 450));
            CreateIndex("dbo.Players", "PersonaName");
        }
    }
}

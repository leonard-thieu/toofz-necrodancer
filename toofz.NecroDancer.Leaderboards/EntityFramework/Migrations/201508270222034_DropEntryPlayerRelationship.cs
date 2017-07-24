namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropEntryPlayerRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Entries", "SteamId", "dbo.Players");
            DropIndex("dbo.Entries", new[] { "SteamId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Entries", "SteamId");
            AddForeignKey("dbo.Entries", "SteamId", "dbo.Players", "SteamId", cascadeDelete: true);
        }
    }
}

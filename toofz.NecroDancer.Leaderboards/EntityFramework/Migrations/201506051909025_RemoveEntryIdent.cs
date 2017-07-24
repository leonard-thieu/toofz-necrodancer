namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEntryIdent : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Entries", "EntryIdent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Entries", "EntryIdent", c => c.Int(nullable: false, identity: true));
        }
    }
}

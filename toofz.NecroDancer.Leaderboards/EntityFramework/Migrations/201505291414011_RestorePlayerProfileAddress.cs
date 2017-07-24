namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestorePlayerProfileAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "ProfileAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "ProfileAddress");
        }
    }
}

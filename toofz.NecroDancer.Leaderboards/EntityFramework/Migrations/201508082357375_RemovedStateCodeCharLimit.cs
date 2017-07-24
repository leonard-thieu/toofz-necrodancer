namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedStateCodeCharLimit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Players", "StateCode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Players", "StateCode", c => c.String(maxLength: 3));
        }
    }
}

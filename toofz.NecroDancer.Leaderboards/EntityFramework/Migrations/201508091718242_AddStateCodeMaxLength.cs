namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStateCodeMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Players", "StateCode", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Players", "StateCode", c => c.String());
        }
    }
}

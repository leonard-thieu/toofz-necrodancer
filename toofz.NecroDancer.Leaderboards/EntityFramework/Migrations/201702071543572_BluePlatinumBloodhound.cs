namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BluePlatinumBloodhound : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Avatar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "Avatar");
        }
    }
}

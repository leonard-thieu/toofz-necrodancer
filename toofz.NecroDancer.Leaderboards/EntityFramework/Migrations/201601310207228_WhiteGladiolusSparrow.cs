namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WhiteGladiolusSparrow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NecrobotEntries",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        Player = c.String(nullable: false, maxLength: 128),
                        Zone = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Time = c.Time(precision: 7),
                    })
                .PrimaryKey(t => new { t.Date, t.Player })
                .ForeignKey("dbo.NecrobotLeaderboards", t => t.Date, cascadeDelete: true)
                .Index(t => t.Date);
            
            CreateTable(
                "dbo.NecrobotLeaderboards",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        Seed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Date);
            
            AddColumn("dbo.Replays", "Seed", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NecrobotEntries", "Date", "dbo.NecrobotLeaderboards");
            DropIndex("dbo.NecrobotEntries", new[] { "Date" });
            DropColumn("dbo.Replays", "Seed");
            DropTable("dbo.NecrobotLeaderboards");
            DropTable("dbo.NecrobotEntries");
        }
    }
}

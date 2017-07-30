namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _LimeZinc﻿AfghanHound : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NecrobotEntries", "Date", "dbo.NecrobotLeaderboards");
            DropIndex("dbo.NecrobotEntries", new[] { "Date" });
            CreateTable(
                "dbo.DailyLeaderboards",
                c => new
                    {
                        LeaderboardId = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                        IsProduction = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LeaderboardId);
            
            CreateTable(
                "dbo.DailyEntries",
                c => new
                    {
                        LeaderboardId = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        SteamId = c.Long(nullable: false),
                        ReplayId = c.Long(),
                        Score = c.Int(nullable: false),
                        Zone = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LeaderboardId, t.Rank })
                .ForeignKey("dbo.DailyLeaderboards", t => t.LeaderboardId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.SteamId, cascadeDelete: true)
                .Index(t => t.LeaderboardId)
                .Index(t => t.SteamId);
            
            DropTable("dbo.NecrobotEntries");
            DropTable("dbo.NecrobotLeaderboards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.NecrobotLeaderboards",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        Seed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Date);
            
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
                .PrimaryKey(t => new { t.Date, t.Player });
            
            DropForeignKey("dbo.DailyEntries", "SteamId", "dbo.Players");
            DropForeignKey("dbo.DailyEntries", "LeaderboardId", "dbo.DailyLeaderboards");
            DropIndex("dbo.DailyEntries", new[] { "SteamId" });
            DropIndex("dbo.DailyEntries", new[] { "LeaderboardId" });
            DropTable("dbo.DailyEntries");
            DropTable("dbo.DailyLeaderboards");
            CreateIndex("dbo.NecrobotEntries", "Date");
            AddForeignKey("dbo.NecrobotEntries", "Date", "dbo.NecrobotLeaderboards", "Date", cascadeDelete: true);
        }
    }
}

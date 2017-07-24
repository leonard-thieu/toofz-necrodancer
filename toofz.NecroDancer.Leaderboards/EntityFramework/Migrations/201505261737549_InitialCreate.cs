namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entries",
                c => new
                    {
                        SteamId = c.Long(nullable: false),
                        LeaderboardId = c.Int(nullable: false),
                        EntryIdent = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        UgcId = c.Long(nullable: false),
                        Details = c.String(),
                    })
                .PrimaryKey(t => new { t.SteamId, t.LeaderboardId })
                .ForeignKey("dbo.Leaderboards", t => t.LeaderboardId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.SteamId, cascadeDelete: true)
                .Index(t => t.SteamId)
                .Index(t => t.LeaderboardId);
            
            CreateTable(
                "dbo.Leaderboards",
                c => new
                    {
                        LeaderboardId = c.Int(nullable: false),
                        LeaderboardIdent = c.Int(nullable: false, identity: true),
                        Character = c.Int(nullable: false),
                        Run = c.Int(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        DisplayName = c.String(),
                        EntryCount = c.Int(nullable: false),
                        SortMethod = c.Int(nullable: false),
                        DisplayType = c.Int(nullable: false),
                        OnlyTrustedWrites = c.Int(nullable: false),
                        OnlyFriendsReads = c.Int(nullable: false),
                        IsCoOp = c.Boolean(nullable: false),
                        IsCustomMusic = c.Boolean(nullable: false),
                        IsDaily = c.Boolean(nullable: false),
                        IsDeathless = c.Boolean(nullable: false),
                        IsProduction = c.Boolean(nullable: false),
                        IsScore = c.Boolean(nullable: false),
                        IsSeeded = c.Boolean(nullable: false),
                        IsSpeed = c.Boolean(nullable: false),
                        TotalLeaderboardEntries = c.Int(nullable: false),
                        EntryStart = c.Int(nullable: false),
                        EntryEnd = c.Int(nullable: false),
                        NextRequestAddress = c.String(),
                        ResultCount = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        AppId = c.Int(nullable: false),
                        AppFriendlyName = c.String(),
                    })
                .PrimaryKey(t => t.LeaderboardId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        SteamId = c.Long(nullable: false),
                        PlayerIdent = c.Int(nullable: false, identity: true),
                        PersonaName = c.String(),
                        ProfileAddress = c.String(),
                        Avatar = c.String(),
                        AvatarMedium = c.String(),
                        AvatarFull = c.String(),
                        PersonaState = c.Int(nullable: false),
                        CommunityVisibilityState = c.Int(nullable: false),
                        ProfileState = c.Int(nullable: false),
                        LastLogOff = c.Int(nullable: false),
                        CommentPermission = c.Int(nullable: false),
                        RealName = c.String(),
                        PrimaryClanId = c.Long(nullable: false),
                        TimeCreated = c.Int(nullable: false),
                        GameId = c.String(),
                        GameServerIP = c.String(),
                        GameServerSteamId = c.Long(nullable: false),
                        GameExtraInfo = c.String(),
                        CountryCode = c.String(),
                        StateCode = c.String(),
                        CityId = c.Int(nullable: false),
                        PersonaStateFlags = c.Int(nullable: false),
                        LobbySteamId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SteamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entries", "SteamId", "dbo.Players");
            DropForeignKey("dbo.Entries", "LeaderboardId", "dbo.Leaderboards");
            DropIndex("dbo.Entries", new[] { "LeaderboardId" });
            DropIndex("dbo.Entries", new[] { "SteamId" });
            DropTable("dbo.Players");
            DropTable("dbo.Leaderboards");
            DropTable("dbo.Entries");
        }
    }
}

namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAllUnusedColumns : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Entries", "UgcId");
            DropColumn("dbo.Leaderboards", "TotalLeaderboardEntries");
            DropColumn("dbo.Leaderboards", "EntryStart");
            DropColumn("dbo.Leaderboards", "EntryEnd");
            DropColumn("dbo.Leaderboards", "NextRequestAddress");
            DropColumn("dbo.Leaderboards", "ResultCount");
            DropColumn("dbo.Leaderboards", "Address");
            DropColumn("dbo.Leaderboards", "DisplayName");
            DropColumn("dbo.Leaderboards", "EntryCount");
            DropColumn("dbo.Leaderboards", "SortMethod");
            DropColumn("dbo.Leaderboards", "DisplayType");
            DropColumn("dbo.Leaderboards", "OnlyTrustedWrites");
            DropColumn("dbo.Leaderboards", "OnlyFriendsReads");
            DropColumn("dbo.Leaderboards", "AppId");
            DropColumn("dbo.Leaderboards", "AppFriendlyName");
            DropColumn("dbo.Players", "ProfileAddress");
            DropColumn("dbo.Players", "Avatar");
            DropColumn("dbo.Players", "AvatarMedium");
            DropColumn("dbo.Players", "AvatarFull");
            DropColumn("dbo.Players", "PersonaState");
            DropColumn("dbo.Players", "CommunityVisibilityState");
            DropColumn("dbo.Players", "ProfileState");
            DropColumn("dbo.Players", "LastLogOff");
            DropColumn("dbo.Players", "CommentPermission");
            DropColumn("dbo.Players", "RealName");
            DropColumn("dbo.Players", "PrimaryClanId");
            DropColumn("dbo.Players", "TimeCreated");
            DropColumn("dbo.Players", "GameId");
            DropColumn("dbo.Players", "GameServerIP");
            DropColumn("dbo.Players", "GameServerSteamId");
            DropColumn("dbo.Players", "GameExtraInfo");
            DropColumn("dbo.Players", "CountryCode");
            DropColumn("dbo.Players", "StateCode");
            DropColumn("dbo.Players", "CityId");
            DropColumn("dbo.Players", "PersonaStateFlags");
            DropColumn("dbo.Players", "LobbySteamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "LobbySteamId", c => c.Long(nullable: false));
            AddColumn("dbo.Players", "PersonaStateFlags", c => c.Int(nullable: false));
            AddColumn("dbo.Players", "CityId", c => c.Int(nullable: false));
            AddColumn("dbo.Players", "StateCode", c => c.String(maxLength: 3));
            AddColumn("dbo.Players", "CountryCode", c => c.String(maxLength: 2));
            AddColumn("dbo.Players", "GameExtraInfo", c => c.String());
            AddColumn("dbo.Players", "GameServerSteamId", c => c.Long(nullable: false));
            AddColumn("dbo.Players", "GameServerIP", c => c.String());
            AddColumn("dbo.Players", "GameId", c => c.String());
            AddColumn("dbo.Players", "TimeCreated", c => c.Int(nullable: false));
            AddColumn("dbo.Players", "PrimaryClanId", c => c.Long(nullable: false));
            AddColumn("dbo.Players", "RealName", c => c.String());
            AddColumn("dbo.Players", "CommentPermission", c => c.Int(nullable: false));
            AddColumn("dbo.Players", "LastLogOff", c => c.Int(nullable: false));
            AddColumn("dbo.Players", "ProfileState", c => c.Int(nullable: false));
            AddColumn("dbo.Players", "CommunityVisibilityState", c => c.Int(nullable: false));
            AddColumn("dbo.Players", "PersonaState", c => c.Int(nullable: false));
            AddColumn("dbo.Players", "AvatarFull", c => c.String());
            AddColumn("dbo.Players", "AvatarMedium", c => c.String());
            AddColumn("dbo.Players", "Avatar", c => c.String());
            AddColumn("dbo.Players", "ProfileAddress", c => c.String());
            AddColumn("dbo.Leaderboards", "AppFriendlyName", c => c.String());
            AddColumn("dbo.Leaderboards", "AppId", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "OnlyFriendsReads", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "OnlyTrustedWrites", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "DisplayType", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "SortMethod", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "EntryCount", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "DisplayName", c => c.String());
            AddColumn("dbo.Leaderboards", "Address", c => c.String());
            AddColumn("dbo.Leaderboards", "ResultCount", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "NextRequestAddress", c => c.String());
            AddColumn("dbo.Leaderboards", "EntryEnd", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "EntryStart", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "TotalLeaderboardEntries", c => c.Int(nullable: false));
            AddColumn("dbo.Entries", "UgcId", c => c.Long(nullable: false));
        }
    }
}

namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FuschiaCopperPomeranian : DbMigration
    {
        private const string EntriesA = "dbo.Entries_A";
        private const string EntriesB = "dbo.Entries_B";

        public override void Up()
        {
            //try
            //{
            //    var pk = "sp_rename '[dbo].[PK_dbo.Entries]', 'PK_dbo.Entries_B', 'OBJECT';";
            //    Sql(pk);
            //}
            //catch (SqlException) { }

            Up(EntriesA);
            Up(EntriesB);

            // To handle rename of UgcId to ReplayId
            var view = @"
EXEC('ALTER VIEW dbo.Entries
AS

SELECT 
    SteamId, LeaderboardId, Score, Rank, Zone, Level, ReplayId
FROM dbo.Entries_A;');";
            Sql(view);

            DropIndex("dbo.Players", new[] { "PersonaName" });
            RenameColumn("dbo.Players", "PersonaName", "Name");
            AlterColumn("dbo.Players", "LastUpdate", c => c.DateTime());

            AddColumn("dbo.Leaderboards", "CharacterId", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "RunId", c => c.Int(nullable: false));
            AddColumn("dbo.Leaderboards", "Date", c => c.DateTime());
            AddColumn("dbo.Leaderboards", "IsCooperative", c => c.Boolean(nullable: false));

            DropColumn("dbo.Leaderboards", "TotalLeaderboardEntries");
            DropColumn("dbo.Leaderboards", "Name");
            DropColumn("dbo.Leaderboards", "IsCoOp");
            DropColumn("dbo.Leaderboards", "IsCustomMusic");
            DropColumn("dbo.Leaderboards", "IsDaily");
            DropColumn("dbo.Leaderboards", "IsDeathless");
            DropColumn("dbo.Leaderboards", "IsScore");
            DropColumn("dbo.Leaderboards", "IsSeeded");
            DropColumn("dbo.Leaderboards", "IsSpeed");

            DropColumn("dbo.Replays", "SteamId");
            DropColumn("dbo.Replays", "LeaderboardId");
            DropColumn("dbo.Replays", "IsDataValid");

            //Sql("CREATE FULLTEXT CATALOG ft AS DEFAULT;", suppressTransaction: true);
            //Sql("CREATE FULLTEXT INDEX ON [dbo].[Players] ([Name] LANGUAGE [Neutral]) KEY INDEX [PK_dbo.Players] ON ([ft]) WITH (STOPLIST OFF);", suppressTransaction: true);
        }

        private void Up(string table)
        {
            DropPrimaryKey(table);
            AddPrimaryKey(table, new[] { "LeaderboardId", "Rank" });

            // Player rows must exist before adding FK on Entry to Player
            var players = $@"
MERGE dbo.Players as Target
USING (SELECT DISTINCT SteamId FROM {table}) AS Source
ON (Target.SteamId = Source.SteamId)
WHEN NOT MATCHED BY TARGET THEN
    INSERT (SteamId)
    VALUES (Source.SteamId);";
            Sql(players);

            AlterColumn(table, "UgcId", c => c.Long(nullable: true));
            RenameColumn(table, "UgcId", "ReplayId");
            CreateIndex(table, "SteamId");

            AddForeignKey(table, "SteamId", "dbo.Players", "SteamId", cascadeDelete: true);
        }

        public override void Down()
        {
            Down(EntriesA);
            Down(EntriesB);

            // To handle rename of ReplayId to UgcId
            var view = @"
EXEC('ALTER VIEW dbo.Entries
AS

SELECT 
    SteamId, LeaderboardId, Score, Rank, Zone, Level, UgcId
FROM dbo.Entries_A;');";
            Sql(view);

            AddColumn("dbo.Replays", "IsDataValid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Replays", "LeaderboardId", c => c.Int(nullable: false));
            AddColumn("dbo.Replays", "SteamId", c => c.Long(nullable: false));

            RenameColumn("dbo.Players", "Name", "PersonaName");

            // LastUpdate becomes non-nullable so set defaults
            var lastUpdate = @"
UPDATE dbo.Players 
SET LastUpdate = GETDATE()
WHERE LastUpdate IS NULL;";
            Sql(lastUpdate);
            AlterColumn("dbo.Players", "LastUpdate", c => c.DateTime(nullable: false));

            CreateIndex("dbo.Players", "PersonaName");

            AddColumn("dbo.Leaderboards", "IsSpeed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "IsSeeded", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "IsScore", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "IsDeathless", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "IsDaily", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "IsCustomMusic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "IsCoOp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "Name", c => c.String());
            AddColumn("dbo.Leaderboards", "TotalLeaderboardEntries", c => c.Int(nullable: false));

            DropColumn("dbo.Leaderboards", "IsCooperative");
            DropColumn("dbo.Leaderboards", "Date");
            DropColumn("dbo.Leaderboards", "RunId");
            DropColumn("dbo.Leaderboards", "CharacterId");

            Sql("DROP FULLTEXT INDEX ON [dbo].[Players];", suppressTransaction: true);
            Sql("DROP FULLTEXT CATALOG ft;", suppressTransaction: true);
        }

        private void Down(string table)
        {
            DropForeignKey(table, "SteamId", "dbo.Players");
            DropIndex(table, new[] { "SteamId" });
            RenameColumn(table, "ReplayId", "UgcId");

            var ugcIds = $@"
UPDATE {table} 
SET UgcId = -1
WHERE UgcId IS NULL;";
            Sql(ugcIds);
            AlterColumn(table, "UgcId", c => c.Long(nullable: false));
            DropPrimaryKey(table);
            AddPrimaryKey(table, new[] { "SteamId", "LeaderboardId" });
        }
    }
}

namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEntriesView : DbMigration
    {
        public override void Up()
        {
            RenameTable("dbo.Entries", "Entries_A");

            var tableB = @"
CREATE TABLE [dbo].[Entries_B](
    [SteamId] [bigint] NOT NULL,
    [LeaderboardId] [int] NOT NULL,
    [Score] [int] NOT NULL,
    [Rank] [int] NOT NULL,
    [Zone] [int] NOT NULL DEFAULT ((0)),
    [Level] [int] NOT NULL DEFAULT ((0)),
    [UgcId] [bigint] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_dbo.Entries_B] PRIMARY KEY CLUSTERED 
(
    [SteamId] ASC,
    [LeaderboardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Entries_B]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Entries_B_dbo.Leaderboards_LeaderboardId] FOREIGN KEY([LeaderboardId])
REFERENCES [dbo].[Leaderboards] ([LeaderboardId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Entries_B] CHECK CONSTRAINT [FK_dbo.Entries_B_dbo.Leaderboards_LeaderboardId]
GO";
            Sql(tableB);
            CreateIndex("dbo.Entries_B", "LeaderboardId");

            var view = @"
CREATE VIEW dbo.Entries
AS

SELECT 
    SteamId, LeaderboardId, Score, Rank, Zone, Level, UgcId
FROM dbo.Entries_A;
GO";
            Sql(view);
        }

        public override void Down()
        {
            var view = @"DROP VIEW dbo.Entries;";
            Sql(view);
            DropTable("dbo.Entries_B");
            RenameTable("dbo.Entries_A", "Entries");
        }
    }
}

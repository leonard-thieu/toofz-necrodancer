namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SilverLilacSquirrel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Exists", c => c.Boolean());
            AddColumn("dbo.Replays", "ErrorCode", c => c.Int());
            AlterColumn("dbo.Replays", "Version", c => c.Int());
            Sql("UPDATE dbo.Replays SET Version = NULL WHERE Version = 0;");
            DropColumn("dbo.Replays", "LocalFileName");
        }

        public override void Down()
        {
            AddColumn("dbo.Replays", "LocalFileName", c => c.String());
            Sql("UPDATE dbo.Replays SET Version = 0 WHERE Version IS NULL;");
            AlterColumn("dbo.Replays", "Version", c => c.Int(nullable: false));
            DropColumn("dbo.Replays", "ErrorCode");
            DropColumn("dbo.Players", "Exists");
        }
    }
}

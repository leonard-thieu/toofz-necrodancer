namespace toofz.NecroDancer.Leaderboards.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AquaJasmineRabbit : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM dbo.Leaderboards WHERE IsProduction = 0 OR IsCooperative = 1;");
            DropColumn("dbo.Leaderboards", "IsProduction");
            DropColumn("dbo.Leaderboards", "IsCooperative");
        }

        public override void Down()
        {
            AddColumn("dbo.Leaderboards", "IsCooperative", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaderboards", "IsProduction", c => c.Boolean(nullable: false));
            Sql("UPDATE dbo.Leaderboards SET IsProduction = 1;");
        }
    }
}

namespace toofz.NecroDancer.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaroonCopperPomeranian : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "FromTransmute", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "HideQuantity", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsAxe", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsCutlass", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsFamiliar", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsHarp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsMagicFood", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsStackable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsStaff", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsTemp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsWarhammer", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Items", "QuantityYOff", c => c.Int(nullable: false));
            AddColumn("dbo.Items", "Set", c => c.String());
            AddColumn("dbo.Items", "TemporaryMapSight", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "UseGreater", c => c.Boolean(nullable: false));
            AddColumn("dbo.Enemies", "OptionalStats_Boss", c => c.Boolean(nullable: false));
            AddColumn("dbo.Enemies", "OptionalStats_BounceOnMovementFail", c => c.Boolean(nullable: false));
            AddColumn("dbo.Enemies", "OptionalStats_Floating", c => c.Boolean(nullable: false));
            AddColumn("dbo.Enemies", "OptionalStats_IgnoreLiquids", c => c.Boolean(nullable: false));
            AddColumn("dbo.Enemies", "OptionalStats_IgnoreWalls", c => c.Boolean(nullable: false));
            AddColumn("dbo.Enemies", "OptionalStats_IsMonkeyLike", c => c.Boolean(nullable: false));
            AddColumn("dbo.Enemies", "OptionalStats_Massive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Enemies", "OptionalStats_Miniboss", c => c.Boolean(nullable: false));
            DropColumn("dbo.Enemies", "OptionalStats");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Enemies", "OptionalStats", c => c.Int(nullable: false));
            DropColumn("dbo.Enemies", "OptionalStats_Miniboss");
            DropColumn("dbo.Enemies", "OptionalStats_Massive");
            DropColumn("dbo.Enemies", "OptionalStats_IsMonkeyLike");
            DropColumn("dbo.Enemies", "OptionalStats_IgnoreWalls");
            DropColumn("dbo.Enemies", "OptionalStats_IgnoreLiquids");
            DropColumn("dbo.Enemies", "OptionalStats_Floating");
            DropColumn("dbo.Enemies", "OptionalStats_BounceOnMovementFail");
            DropColumn("dbo.Enemies", "OptionalStats_Boss");
            DropColumn("dbo.Items", "UseGreater");
            DropColumn("dbo.Items", "TemporaryMapSight");
            DropColumn("dbo.Items", "Set");
            DropColumn("dbo.Items", "QuantityYOff");
            DropColumn("dbo.Items", "Quantity");
            DropColumn("dbo.Items", "IsWarhammer");
            DropColumn("dbo.Items", "IsTemp");
            DropColumn("dbo.Items", "IsStaff");
            DropColumn("dbo.Items", "IsStackable");
            DropColumn("dbo.Items", "IsMagicFood");
            DropColumn("dbo.Items", "IsHarp");
            DropColumn("dbo.Items", "IsFamiliar");
            DropColumn("dbo.Items", "IsCutlass");
            DropColumn("dbo.Items", "IsAxe");
            DropColumn("dbo.Items", "HideQuantity");
            DropColumn("dbo.Items", "FromTransmute");
        }
    }
}

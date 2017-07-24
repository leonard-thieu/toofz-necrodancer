namespace toofz.NecroDancer.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ElementName = c.String(nullable: false, maxLength: 128),
                        Bouncer = c.Boolean(nullable: false),
                        ChestChance = c.String(),
                        CoinCost = c.Int(),
                        Consumable = c.Boolean(nullable: false),
                        Cooldown = c.Int(nullable: false),
                        Data = c.Int(nullable: false),
                        DiamondCost = c.Int(),
                        DiamondDealable = c.Int(nullable: false),
                        Flyaway_Id = c.Int(nullable: false),
                        Flyaway_Text = c.String(),
                        FrameCount = c.Int(nullable: false),
                        Hint_Id = c.Int(nullable: false),
                        Hint_Text = c.String(),
                        ImageHeight = c.Int(nullable: false),
                        ImageWidth = c.Int(nullable: false),
                        IsArmor = c.Boolean(nullable: false),
                        IsBlood = c.Boolean(nullable: false),
                        IsBlunderbuss = c.Boolean(nullable: false),
                        IsBow = c.Boolean(nullable: false),
                        IsBroadsword = c.Boolean(nullable: false),
                        IsCat = c.Boolean(nullable: false),
                        IsCoin = c.Boolean(nullable: false),
                        IsCrossbow = c.Boolean(nullable: false),
                        IsDagger = c.Boolean(nullable: false),
                        IsDiamond = c.Boolean(nullable: false),
                        IsFlail = c.Boolean(nullable: false),
                        IsFood = c.Boolean(nullable: false),
                        IsFrost = c.Boolean(nullable: false),
                        IsGlass = c.Boolean(nullable: false),
                        IsGold = c.Boolean(nullable: false),
                        IsLongsword = c.Boolean(nullable: false),
                        IsObsidian = c.Boolean(nullable: false),
                        IsPhasing = c.Boolean(nullable: false),
                        IsPiercing = c.Boolean(nullable: false),
                        IsRapier = c.Boolean(nullable: false),
                        IsRifle = c.Boolean(nullable: false),
                        IsScroll = c.Boolean(nullable: false),
                        IsShovel = c.Boolean(nullable: false),
                        IsSpear = c.Boolean(nullable: false),
                        IsSpell = c.Boolean(nullable: false),
                        IsTitanium = c.Boolean(nullable: false),
                        IsTorch = c.Boolean(nullable: false),
                        IsWeapon = c.Boolean(nullable: false),
                        IsWhip = c.Boolean(nullable: false),
                        LevelEditor = c.Boolean(nullable: false),
                        LockedChestChance = c.String(),
                        LockedShopChance = c.String(),
                        OffsetY = c.Int(nullable: false),
                        PlayerKnockback = c.Boolean(nullable: false),
                        ScreenFlash = c.Boolean(nullable: false),
                        ScreenShake = c.Boolean(nullable: false),
                        ShopChance = c.String(),
                        Slot = c.String(),
                        SlotPriority = c.Int(nullable: false),
                        Sound = c.String(),
                        Spell = c.String(),
                        Unlocked = c.Boolean(nullable: false),
                        UrnChance = c.String(),
                        ImagePath = c.String(),
                        Name = c.String(maxLength: 450),
                    })
                .PrimaryKey(t => t.ElementName)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.Enemies",
                c => new
                    {
                        ElementName = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                        Id = c.Int(),
                        FriendlyName = c.String(),
                        LevelEditor = c.Boolean(nullable: false),
                        SpriteSheet_Path = c.String(),
                        SpriteSheet_FrameCount = c.Int(nullable: false),
                        SpriteSheet_FrameWidth = c.Int(nullable: false),
                        SpriteSheet_FrameHeight = c.Int(nullable: false),
                        SpriteSheet_OffsetX = c.Int(nullable: false),
                        SpriteSheet_OffsetY = c.Int(nullable: false),
                        SpriteSheet_OffsetZ = c.Int(nullable: false),
                        SpriteSheet_HeartOffsetX = c.Int(nullable: false),
                        SpriteSheet_HeartOffsetY = c.Int(nullable: false),
                        Shadow_Path = c.String(),
                        Shadow_OffsetX = c.Int(nullable: false),
                        Shadow_OffsetY = c.Int(nullable: false),
                        Stats_BeatsPerMove = c.Int(nullable: false),
                        Stats_CoinsToDrop = c.Int(nullable: false),
                        Stats_DamagePerHit = c.Int(nullable: false),
                        Stats_Health = c.Int(nullable: false),
                        Stats_Movement = c.String(),
                        Stats_Priority = c.Int(),
                        OptionalStats = c.Int(nullable: false),
                        Bouncer_Min = c.Double(nullable: false),
                        Bouncer_Max = c.Double(nullable: false),
                        Bouncer_Power = c.Double(nullable: false),
                        Bouncer_Steps = c.Int(nullable: false),
                        Tweens_Move = c.String(),
                        Tweens_MoveShadow = c.String(),
                        Tweens_Hit = c.String(),
                        Tweens_HitShadow = c.String(),
                        Particle_HitPath = c.String(),
                        Name = c.String(maxLength: 450),
                    })
                .PrimaryKey(t => new { t.ElementName, t.Type })
                .Index(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Enemies", new[] { "Name" });
            DropIndex("dbo.Items", new[] { "Name" });
            DropTable("dbo.Enemies");
            DropTable("dbo.Items");
        }
    }
}

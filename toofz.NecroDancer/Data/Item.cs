using System.Diagnostics;

namespace toofz.NecroDancer.Data
{
    [DebuggerDisplay("{Name}")]
    public sealed class Item
    {
        public string ElementName { get; set; }

        public bool Bouncer { get; set; }
        public string ChestChance { get; set; }
        public int? CoinCost { get; set; }
        public bool Consumable { get; set; }
        public int Cooldown { get; set; }
        public int Data { get; set; }
        public int? DiamondCost { get; set; }
        public int DiamondDealable { get; set; }
        public DisplayString Flyaway { get; set; } = DisplayString.Empty;
        public int FrameCount { get; set; } = 1;
        public bool FromTransmute { get; set; }
        public bool HideQuantity { get; set; }
        public DisplayString Hint { get; set; } = DisplayString.Empty;
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public bool IsArmor { get; set; }
        public bool IsAxe { get; set; }
        public bool IsBlood { get; set; }
        public bool IsBlunderbuss { get; set; }
        public bool IsBow { get; set; }
        public bool IsBroadsword { get; set; }
        public bool IsCat { get; set; }
        public bool IsCoin { get; set; }
        public bool IsCrossbow { get; set; }
        public bool IsCutlass { get; set; }
        public bool IsDagger { get; set; }
        public bool IsDiamond { get; set; }
        public bool IsFamiliar { get; set; }
        public bool IsFlail { get; set; }
        public bool IsFood { get; set; }
        public bool IsFrost { get; set; }
        public bool IsGlass { get; set; }
        public bool IsGold { get; set; }
        public bool IsHarp { get; set; }
        public bool IsLongsword { get; set; }
        public bool IsMagicFood { get; set; }
        public bool IsObsidian { get; set; }
        public bool IsPhasing { get; set; }
        public bool IsPiercing { get; set; }
        public bool IsRapier { get; set; }
        public bool IsRifle { get; set; }
        public bool IsScroll { get; set; }
        public bool IsShovel { get; set; }
        public bool IsSpear { get; set; }
        public bool IsSpell { get; set; }
        public bool IsStackable { get; set; }
        public bool IsStaff { get; set; }
        public bool IsTemp { get; set; }
        public bool IsTitanium { get; set; }
        public bool IsTorch { get; set; }
        public bool IsWarhammer { get; set; }
        public bool IsWeapon { get; set; }
        public bool IsWhip { get; set; }
        public bool LevelEditor { get; set; } = true;
        public string LockedChestChance { get; set; }
        public string LockedShopChance { get; set; }
        public int OffsetY { get; set; }
        public bool PlayerKnockback { get; set; }
        public int Quantity { get; set; }
        public int QuantityYOff { get; set; }
        public bool ScreenFlash { get; set; }
        public bool ScreenShake { get; set; }
        public string Set { get; set; }
        public string ShopChance { get; set; }
        public string Slot { get; set; }
        public int SlotPriority { get; set; }
        public string Sound { get; set; }
        public string Spell { get; set; }
        public bool TemporaryMapSight { get; set; }
        public bool Unlocked { get; set; }
        public string UrnChance { get; set; }
        public bool UseGreater { get; set; }

        public string ImagePath { get; set; }
        public string Name { get; set; }
    }
}

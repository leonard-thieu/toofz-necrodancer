using System.Xml.Serialization;

namespace toofz.NecroDancer.Saves
{
    public sealed class Player
    {
        [XmlAttribute("addchest_blackUnlocked")]
        public bool IsAddBlackChestUnlocked { get; set; }
        [XmlAttribute("addchest_redUnlocked")]
        public bool IsAddRedChestUnlocked { get; set; }
        [XmlAttribute("addchest_whiteUnlocked")]
        public bool IsAddWhiteChestUnlocked { get; set; }
        [XmlAttribute("armor_chainmailUnlocked")]
        public bool IsChainmailUnlocked { get; set; }
        [XmlAttribute("armor_giUnlocked")]
        public bool IsGiUnlocked { get; set; }
        [XmlAttribute("armor_glassUnlocked")]
        public bool IsGlassArmorUnlocked { get; set; }
        [XmlAttribute("armor_heavyplateUnlocked")]
        public bool IsHeavyPlateUnlocked { get; set; }
        [XmlAttribute("armor_obsidianUnlocked")]
        public bool IsObsidianArmorUnlocked { get; set; }
        [XmlAttribute("armor_platemailUnlocked")]
        public bool IsPlatemailUnlocked { get; set; }
        [XmlAttribute("bag_holdingUnlocked")]
        public bool IsBagOfHoldingUnlocked { get; set; }
        [XmlAttribute("coins_x15Unlocked")]
        public bool IsCoin15MultiplierUnlocked { get; set; }
        [XmlAttribute("coins_x2Unlocked")]
        public bool IsCoin20MultiplierUnlocked { get; set; }
        [XmlAttribute("diamondDealerItems")]
        public string DiamondDealerItems { get; set; }
        [XmlAttribute("diamondDealerSoldItem1")]
        public string DiamondDealerSoldItem1 { get; set; }
        [XmlAttribute("diamondDealerSoldItem2")]
        public string DiamondDealerSoldItem2 { get; set; }
        [XmlAttribute("diamondDealerSoldItem3")]
        public string DiamondDealerSoldItem3 { get; set; }
        [XmlAttribute("feet_greavesUnlocked")]
        public bool IsGreavesUnlocked { get; set; }
        [XmlAttribute("food_2Unlocked")]
        public bool IsFood2Unlocked { get; set; }
        [XmlAttribute("food_3Unlocked")]
        public bool IsFood3Unlocked { get; set; }
        [XmlAttribute("food_4Unlocked")]
        public bool IsFood4Unlocked { get; set; }
        [XmlAttribute("head_crown_of_thornsUnlocked")]
        public bool IsCrownOfThornsUnlocked { get; set; }
        [XmlAttribute("head_helmUnlocked")]
        public bool IsHelmUnlocked { get; set; }
        [XmlAttribute("heart_transplantUnlocked")]
        public bool IsHeartTransplantUnlocked { get; set; }
        [XmlAttribute("holsterUnlocked")]
        public bool IsHolsterUnlocked { get; set; }
        [XmlAttribute("maxHealth")]
        public int MaxHealth { get; set; }
        [XmlAttribute("numCoins")]
        public int CoinCount { get; set; }
        [XmlAttribute("numDiamonds")]
        public int DiamondCount { get; set; }
        [XmlAttribute("pickaxeUnlocked")]
        public bool IsPickaxeUnlocked { get; set; }
        [XmlAttribute("ring_courageUnlocked")]
        public bool IsRingOfCourageUnlocked { get; set; }
        [XmlAttribute("ring_goldUnlocked")]
        public bool IsRingOfGoldUnlocked { get; set; }
        [XmlAttribute("ring_luckUnlocked")]
        public bool IsRingOfLuckUnlocked { get; set; }
        [XmlAttribute("ring_manaUnlocked")]
        public bool IsRingOfManaUnlocked { get; set; }
        [XmlAttribute("ring_mightUnlocked")]
        public bool IsRingOfMightUnlocked { get; set; }
        [XmlAttribute("ring_peaceUnlocked")]
        public bool IsRingOfPeaceUnlocked { get; set; }
        [XmlAttribute("ring_phasingUnlocked")]
        public bool IsRingOfPhasingUnlocked { get; set; }
        [XmlAttribute("ring_protectionUnlocked")]
        public bool IsRingOfProtectionUnlocked { get; set; }
        [XmlAttribute("ring_regenerationUnlocked")]
        public bool IsRingOfRegenerationUnlocked { get; set; }
        [XmlAttribute("ring_shadowsUnlocked")]
        public bool IsRingOfShadowsUnlocked { get; set; }
        [XmlAttribute("ring_warUnlocked")]
        public bool IsRingOfWarUnlocked { get; set; }
        [XmlAttribute("scroll_descendUnlocked")]
        public bool IsScrollOfDescendUnlocked { get; set; }
        [XmlAttribute("scroll_earthquakeUnlocked")]
        public bool IsScrollOfEarthquakeUnlocked { get; set; }
        [XmlAttribute("scroll_enchant_weaponUnlocked")]
        public bool IsScrollOfEnchantWeaponUnlocked { get; set; }
        [XmlAttribute("scroll_fearUnlocked")]
        public bool IsScrollOfFearUnlocked { get; set; }
        [XmlAttribute("scroll_needUnlocked")]
        public bool IsScrollOfNeedUnlocked { get; set; }
        [XmlAttribute("scroll_richesUnlocked")]
        public bool IsScrollOfRichesUnlocked { get; set; }
        [XmlAttribute("shovel_glassUnlocked")]
        public bool IsGlassShovelUnlocked { get; set; }
        [XmlAttribute("shovel_obsidianUnlocked")]
        public bool IsObsidianShovelUnlocked { get; set; }
        [XmlAttribute("spell_bombUnlocked")]
        public bool IsBombSpellUnlocked { get; set; }
        [XmlAttribute("spell_freeze_enemiesUnlocked")]
        public bool IsFreezeSpellUnlocked { get; set; }
        [XmlAttribute("spell_healUnlocked")]
        public bool IsHealSpellUnlocked { get; set; }
        [XmlAttribute("spell_shieldUnlocked")]
        public bool IsShieldSpellUnlocked { get; set; }
        [XmlAttribute("torch_2Unlocked")]
        public bool IsTorch2Unlocked { get; set; }
        [XmlAttribute("torch_3Unlocked")]
        public bool IsTorch3Unlocked { get; set; }
        [XmlAttribute("v")]
        public int V { get; set; }
        [XmlAttribute("war_drumUnlocked")]
        public bool IsWarDrumUnlocked { get; set; }
        [XmlAttribute("weapon_blood_longswordUnlocked")]
        public bool IsBloodLongswordUnlocked { get; set; }
        [XmlAttribute("weapon_blood_rapierUnlocked")]
        public bool IsBloodRapierUnlocked { get; set; }
        [XmlAttribute("weapon_blood_spearUnlocked")]
        public bool IsBloodSpearUnlocked { get; set; }
        [XmlAttribute("weapon_blood_whipUnlocked")]
        public bool IsBloodWhipUnlocked { get; set; }
        [XmlAttribute("weapon_blunderbussUsed")]
        public bool IsBlunderbussUnlocked { get; set; }
        [XmlAttribute("weapon_bowUsed")]
        public bool IsBowUnlocked { get; set; }
        [XmlAttribute("weapon_broadswordUsed")]
        public bool IsBroadswordUnlocked { get; set; }
        [XmlAttribute("weapon_catUnlocked")]
        public bool IsCatUnlocked { get; set; }
        [XmlAttribute("weapon_catUsed")]
        public bool IsCatUsed { get; set; }
        [XmlAttribute("weapon_crossbowUsed")]
        public bool IsCrossbowUnlocked { get; set; }
        [XmlAttribute("weapon_daggerUsed")]
        public bool IsDaggerUnlocked { get; set; }
        [XmlAttribute("weapon_dagger_frostUsed")]
        public bool IsDaggerOfFrostUnlocked { get; set; }
        [XmlAttribute("weapon_dagger_phasingUsed")]
        public bool IsDaggerOfPhasingUnlocked { get; set; }
        [XmlAttribute("weapon_flailUnlocked")]
        public bool IsFlailUnlocked { get; set; }
        [XmlAttribute("weapon_flailUsed")]
        public bool IsFlailUsed { get; set; }
        [XmlAttribute("weapon_glass_longswordUnlocked")]
        public bool IsGlassLongswordUnlocked { get; set; }
        [XmlAttribute("weapon_glass_rapierUnlocked")]
        public bool IsGlassRapierUnlocked { get; set; }
        [XmlAttribute("weapon_glass_spearUnlocked")]
        public bool IsGlassSpearUnlocked { get; set; }
        [XmlAttribute("weapon_glass_whipUnlocked")]
        public bool IsGlassWhipUnlocked { get; set; }
        [XmlAttribute("weapon_golden_longswordUnlocked")]
        public bool IsGoldenLongswordUnlocked { get; set; }
        [XmlAttribute("weapon_golden_rapierUnlocked")]
        public bool IsGoldenRapierUnlocked { get; set; }
        [XmlAttribute("weapon_golden_spearUnlocked")]
        public bool IsGoldenSpearUnlocked { get; set; }
        [XmlAttribute("weapon_golden_whipUnlocked")]
        public bool IsGoldenWhipUnlocked { get; set; }
        [XmlAttribute("weapon_longswordUnlocked")]
        public bool IsLongswordUnlocked { get; set; }
        [XmlAttribute("weapon_longswordUsed")]
        public bool IsLongswordUsed { get; set; }
        [XmlAttribute("weapon_obsidian_longswordUnlocked")]
        public bool IsObsidianLongswordUnlocked { get; set; }
        [XmlAttribute("weapon_obsidian_rapierUnlocked")]
        public bool IsObsidianRapierUnlocked { get; set; }
        [XmlAttribute("weapon_obsidian_spearUnlocked")]
        public bool IsObsidianSpearUnlocked { get; set; }
        [XmlAttribute("weapon_obsidian_whipUnlocked")]
        public bool IsObsidianWhipUnlocked { get; set; }
        [XmlAttribute("weapon_rapierUnlocked")]
        public bool IsRapierUnlocked { get; set; }
        [XmlAttribute("weapon_rapierUsed")]
        public bool IsRapierUsed { get; set; }
        [XmlAttribute("weapon_rifleUsed")]
        public bool IsRifleUnlocked { get; set; }
        [XmlAttribute("weapon_spearUnlocked")]
        public bool IsSpearUnlocked { get; set; }
        [XmlAttribute("weapon_spearUsed")]
        public bool IsSpearUsed { get; set; }
        [XmlAttribute("weapon_titanium_longswordUnlocked")]
        public bool IsTitaniumLongswordUnlocked { get; set; }
        [XmlAttribute("weapon_titanium_rapierUnlocked")]
        public bool IsTitaniumRapierUnlocked { get; set; }
        [XmlAttribute("weapon_titanium_spearUnlocked")]
        public bool IsTitaniumSpearUnlocked { get; set; }
        [XmlAttribute("weapon_titanium_whipUnlocked")]
        public bool IsTitaniumWhipUnlocked { get; set; }
        [XmlAttribute("weapon_whipUnlocked")]
        public bool IsWhipUnlocked { get; set; }
        [XmlAttribute("weapon_whipUsed")]
        public bool IsWhipUsed { get; set; }
    }
}

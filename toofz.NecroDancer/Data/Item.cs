using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using toofz.Xml;

namespace toofz.NecroDancer.Data
{
    [DebuggerDisplay("{Name}")]
    public sealed class Item : IXmlSerializable
    {
        public const string XmlArrayName = "items";

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
        public DisplayString Hint { get; set; } = DisplayString.Empty;
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public bool IsArmor { get; set; }
        public bool IsBlood { get; set; }
        public bool IsBlunderbuss { get; set; }
        public bool IsBow { get; set; }
        public bool IsBroadsword { get; set; }
        public bool IsCat { get; set; }
        public bool IsCoin { get; set; }
        public bool IsCrossbow { get; set; }
        public bool IsDagger { get; set; }
        public bool IsDiamond { get; set; }
        public bool IsFlail { get; set; }
        public bool IsFood { get; set; }
        public bool IsFrost { get; set; }
        public bool IsGlass { get; set; }
        public bool IsGold { get; set; }
        public bool IsLongsword { get; set; }
        public bool IsObsidian { get; set; }
        public bool IsPhasing { get; set; }
        public bool IsPiercing { get; set; }
        public bool IsRapier { get; set; }
        public bool IsRifle { get; set; }
        public bool IsScroll { get; set; }
        public bool IsShovel { get; set; }
        public bool IsSpear { get; set; }
        public bool IsSpell { get; set; }
        public bool IsTitanium { get; set; }
        public bool IsTorch { get; set; }
        public bool IsWeapon { get; set; }
        public bool IsWhip { get; set; }
        public bool LevelEditor { get; set; } = true;
        public string LockedChestChance { get; set; }
        public string LockedShopChance { get; set; }
        public int OffsetY { get; set; }
        public bool PlayerKnockback { get; set; }
        public bool ScreenFlash { get; set; }
        public bool ScreenShake { get; set; }
        public string ShopChance { get; set; }
        public string Slot { get; set; }
        public int SlotPriority { get; set; }
        public string Sound { get; set; }
        public string Spell { get; set; }
        public bool Unlocked { get; set; }
        public string UrnChance { get; set; }

        public string ImagePath { get; set; }
        public string Name { get; set; }

        #region IXmlSerializable Members

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            ElementName = reader.LocalName;

            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "bouncer": Bouncer = reader.ReadContentAsStringAsBoolean(); break;
                    case "chestChance": ChestChance = reader.ReadContentAsString(); break;
                    case "coinCost": CoinCost = reader.ReadContentAsInt(); break;
                    case "consumable": Consumable = reader.ReadContentAsStringAsBoolean(); break;
                    case "cooldown": Cooldown = reader.ReadContentAsInt(); break;
                    case "data": Data = reader.ReadContentAsInt(); break;
                    case "diamondCost": DiamondCost = reader.ReadContentAsInt(); break;
                    case "diamondDealable": DiamondDealable = reader.ReadContentAsInt(); break;
                    case "flyaway": Flyaway = reader.ReadContentAsDisplayString(); break;
                    case "hint": Hint = reader.ReadContentAsDisplayString(); break;
                    case "imageH": ImageHeight = reader.ReadContentAsInt(); break;
                    case "imageW": ImageWidth = reader.ReadContentAsInt(); break;
                    case "isArmor": IsArmor = reader.ReadContentAsStringAsBoolean(); break;
                    case "isBlood": IsBlood = reader.ReadContentAsStringAsBoolean(); break;
                    case "isBlunderbuss": IsBlunderbuss = reader.ReadContentAsStringAsBoolean(); break;
                    case "isBow": IsBow = reader.ReadContentAsStringAsBoolean(); break;
                    case "isBroadsword": IsBroadsword = reader.ReadContentAsStringAsBoolean(); break;
                    case "isCat": IsCat = reader.ReadContentAsStringAsBoolean(); break;
                    case "isCoin": IsCoin = reader.ReadContentAsStringAsBoolean(); break;
                    case "isCrossbow": IsCrossbow = reader.ReadContentAsStringAsBoolean(); break;
                    case "isDagger": IsDagger = reader.ReadContentAsStringAsBoolean(); break;
                    case "isDiamond": IsDiamond = reader.ReadContentAsStringAsBoolean(); break;
                    case "isFlail": IsFlail = reader.ReadContentAsStringAsBoolean(); break;
                    case "isFood": IsFood = reader.ReadContentAsStringAsBoolean(); break;
                    case "isFrost": IsFrost = reader.ReadContentAsStringAsBoolean(); break;
                    case "isGlass": IsGlass = reader.ReadContentAsStringAsBoolean(); break;
                    case "isGold": IsGold = reader.ReadContentAsStringAsBoolean(); break;
                    case "isLongsword": IsLongsword = reader.ReadContentAsStringAsBoolean(); break;
                    case "isObsidian": IsObsidian = reader.ReadContentAsStringAsBoolean(); break;
                    case "isPhasing": IsPhasing = reader.ReadContentAsStringAsBoolean(); break;
                    case "isPiercing": IsPiercing = reader.ReadContentAsStringAsBoolean(); break;
                    case "isRapier": IsRapier = reader.ReadContentAsStringAsBoolean(); break;
                    case "isRifle": IsRifle = reader.ReadContentAsStringAsBoolean(); break;
                    case "isScroll": IsScroll = reader.ReadContentAsStringAsBoolean(); break;
                    case "isShovel": IsShovel = reader.ReadContentAsStringAsBoolean(); break;
                    case "isSpear": IsSpear = reader.ReadContentAsStringAsBoolean(); break;
                    case "isSpell": IsSpell = reader.ReadContentAsStringAsBoolean(); break;
                    case "isTitanium": IsTitanium = reader.ReadContentAsStringAsBoolean(); break;
                    case "isTorch": IsTorch = reader.ReadContentAsStringAsBoolean(); break;
                    case "isWeapon": IsWeapon = reader.ReadContentAsStringAsBoolean(); break;
                    case "isWhip": IsWhip = reader.ReadContentAsStringAsBoolean(); break;
                    case "levelEditor": LevelEditor = reader.ReadContentAsStringAsBoolean(); break;
                    case "lockedChestChance": LockedChestChance = reader.ReadContentAsString(); break;
                    case "lockedShopChance": LockedShopChance = reader.ReadContentAsString(); break;
                    case "numFrames": FrameCount = reader.ReadContentAsInt(); break;
                    case "playerKnockback": PlayerKnockback = reader.ReadContentAsStringAsBoolean(); break;
                    case "screenFlash": ScreenFlash = reader.ReadContentAsStringAsBoolean(); break;
                    case "screenShake": ScreenShake = reader.ReadContentAsStringAsBoolean(); break;
                    case "shopChance": ShopChance = reader.ReadContentAsString(); break;
                    case "slot": Slot = reader.ReadContentAsString(); break;
                    case "slotPriority": SlotPriority = reader.ReadContentAsInt(); break;
                    case "sound": Sound = reader.ReadContentAsString(); break;
                    case "spell": Spell = reader.ReadContentAsString(); break;
                    case "unlocked": Unlocked = reader.ReadContentAsStringAsBoolean(); break;
                    case "urnChance": UrnChance = reader.ReadContentAsString(); break;
                    case "yOff": OffsetY = reader.ReadContentAsInt(); break;
                    default:
                        Trace.TraceWarning($"Unknown attribute '{reader.LocalName}'.");
                        break;
                }
            }

            reader.ReadStartElement(ElementName);

            ImagePath = NormalizeImagePath(reader.Value);
            Name = GetName();

            reader.FindAndReadEndElement();
        }

        private string GetName()
        {
            if (Flyaway == null)
                throw new ArgumentNullException(nameof(Flyaway));

            var name = !Flyaway.Equals(DisplayString.Empty) ? Flyaway.Text : ElementName;

            return name.ToTitleCase();
        }

        private static string NormalizeImagePath(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            return Path.Combine("items", path).Replace('\\', '/');
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

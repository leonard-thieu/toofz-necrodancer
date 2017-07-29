using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using log4net;

namespace toofz.NecroDancer.Data
{
    public static class NecroDancerDataSerializer
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(NecroDancerDataSerializer));

        public static NecroDancerData Read(string uri)
        {
            var doc = XDocument.Load(uri);

            return Read(doc);
        }

        static NecroDancerData Read(XDocument doc)
        {
            var rootEl = doc.Element("necrodancer") ??
                throw new XmlException("Unable to find the root element 'necrodancer'.");
            var data = new NecroDancerData();

            foreach (var rootElEl in rootEl.Elements())
            {
                var rootElElName = rootElEl.Name.ToString();
                switch (rootElElName)
                {
                    case "items":
                        {
                            var itemsEl = rootElEl;
                            var items = data.Items;
                            foreach (var itemEl in itemsEl.Elements())
                            {
                                var itemElName = itemEl.Name.ToString();
                                var item = new Item
                                {
                                    ElementName = itemElName,
                                    ImagePath = itemEl.Value,
                                };

                                foreach (var itemAttr in itemEl.Attributes())
                                {
                                    var itemAttrName = itemAttr.Name.ToString();
                                    switch (itemAttrName)
                                    {
                                        case "bouncer": item.Bouncer = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "chestChance": item.ChestChance = itemAttr.Value; break;
                                        case "coinCost": item.CoinCost = int.Parse(itemAttr.Value); break;
                                        case "consumable": item.Consumable = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "cooldown": item.Cooldown = int.Parse(itemAttr.Value); break;
                                        case "data": item.Data = int.Parse(itemAttr.Value); break;
                                        case "diamondCost": item.DiamondCost = int.Parse(itemAttr.Value); break;
                                        case "diamondDealable": item.DiamondDealable = int.Parse(itemAttr.Value); break;
                                        case "flyaway": item.Flyaway = new DisplayString(itemAttr.Value); break;
                                        case "fromTransmute": item.FromTransmute = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "hideQuantity": item.HideQuantity = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "hint": item.Hint = new DisplayString(itemAttr.Value); break;
                                        case "imageH": item.ImageHeight = int.Parse(itemAttr.Value); break;
                                        case "imageW": item.ImageWidth = int.Parse(itemAttr.Value); break;
                                        case "isArmor": item.IsArmor = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isAxe": item.IsAxe = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isBlood": item.IsBlood = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isBlunderbuss": item.IsBlunderbuss = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isBow": item.IsBow = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isBroadsword": item.IsBroadsword = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isCat": item.IsCat = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isCoin": item.IsCoin = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isCrossbow": item.IsCrossbow = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isCutlass": item.IsCutlass = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isDagger": item.IsDagger = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isDiamond": item.IsDiamond = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isFamiliar": item.IsFamiliar = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isFlail": item.IsFlail = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isFood": item.IsFood = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isFrost": item.IsFrost = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isGlass": item.IsGlass = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isGold": item.IsGold = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isHarp": item.IsHarp = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isLongsword": item.IsLongsword = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isMagicFood": item.IsMagicFood = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isObsidian": item.IsObsidian = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isPhasing": item.IsPhasing = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isPiercing": item.IsPiercing = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isRapier": item.IsRapier = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isRifle": item.IsRifle = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isScroll": item.IsScroll = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isShovel": item.IsShovel = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isSpear": item.IsSpear = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isSpell": item.IsSpell = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isStackable": item.IsStackable = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isStaff": item.IsStaff = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isTemp": item.IsTemp = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isTitanium": item.IsTitanium = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isTorch": item.IsTorch = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isWarhammer": item.IsWarhammer = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isWeapon": item.IsWeapon = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "isWhip": item.IsWhip = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "levelEditor": item.LevelEditor = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "lockedChestChance": item.LockedChestChance = itemAttr.Value; break;
                                        case "lockedShopChance": item.LockedShopChance = itemAttr.Value; break;
                                        case "numFrames": item.FrameCount = int.Parse(itemAttr.Value); break;
                                        case "playerKnockback": item.PlayerKnockback = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "quantity": item.Quantity = int.Parse(itemAttr.Value); break;
                                        case "quantityYOff": item.QuantityYOff = int.Parse(itemAttr.Value); break;
                                        case "screenFlash": item.ScreenFlash = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "screenShake": item.ScreenShake = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "set": item.Set = itemAttr.Value; break;
                                        case "shopChance": item.ShopChance = itemAttr.Value; break;
                                        case "slot": item.Slot = itemAttr.Value; break;
                                        case "slotPriority": item.SlotPriority = int.Parse(itemAttr.Value); break;
                                        case "sound": item.Sound = itemAttr.Value; break;
                                        case "spell": item.Spell = itemAttr.Value; break;
                                        case "temporaryMapSight": item.TemporaryMapSight = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "unlocked": item.Unlocked = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "urnChance": item.UrnChance = itemAttr.Value; break;
                                        case "useGreater": item.UseGreater = ReadContentAsStringAsBoolean(itemAttr.Value); break;
                                        case "yOff": item.OffsetY = int.Parse(itemAttr.Value); break;
                                        default: Log.Debug($"Unknown item attribute: '{itemAttrName}'."); break;
                                    }
                                }

                                item.Name = GetName(item.Flyaway, item.ElementName);

                                items.Add(item);
                            }
                            break;
                        }
                    case "enemies":
                        {
                            var enemiesEl = rootElEl;
                            var enemies = data.Enemies;
                            foreach (var enemyEl in enemiesEl.Elements())
                            {
                                var enemyElName = enemyEl.Name.ToString();
                                var enemy = new Enemy
                                {
                                    ElementName = enemyElName,
                                };

                                foreach (var enemyAttr in enemyEl.Attributes())
                                {
                                    var enemyAttrName = enemyAttr.Name.ToString();
                                    switch (enemyAttrName)
                                    {
                                        case "friendlyName": enemy.FriendlyName = enemyAttr.Value; break;
                                        case "id": enemy.Id = int.Parse(enemyAttr.Value); break;
                                        case "levelEditor": enemy.LevelEditor = ReadContentAsStringAsBoolean(enemyAttr.Value); break;
                                        case "type": enemy.Type = int.Parse(enemyAttr.Value); break;
                                        default: Log.Debug($"Unknown enemy attribute: '{enemyAttrName}'."); break;
                                    }
                                }

                                foreach (var enemyElEl in enemyEl.Elements())
                                {
                                    var enemyElElName = enemyElEl.Name.ToString();
                                    switch (enemyElElName)
                                    {
                                        case "bouncer":
                                            {
                                                var bouncerEl = enemyElEl;
                                                var bouncer = enemy.Bouncer;
                                                foreach (var bouncerAttr in bouncerEl.Attributes())
                                                {
                                                    var bouncerAttrName = bouncerAttr.Name.ToString();
                                                    switch (bouncerAttrName)
                                                    {
                                                        case "min": bouncer.Min = double.Parse(bouncerAttr.Value); break;
                                                        case "max": bouncer.Max = double.Parse(bouncerAttr.Value); break;
                                                        case "power": bouncer.Power = double.Parse(bouncerAttr.Value); break;
                                                        case "steps": bouncer.Steps = int.Parse(bouncerAttr.Value); break;
                                                        default: Log.Debug($"Unknown bouncer attribute: '{bouncerAttrName}'."); break;
                                                    }
                                                }
                                                break;
                                            }
                                        case "frame":
                                            {
                                                var frameEl = enemyElEl;
                                                var frame = new Frame();
                                                foreach (var frameAttr in frameEl.Attributes())
                                                {
                                                    var frameAttrName = frameAttr.Name.ToString();
                                                    switch (frameAttrName)
                                                    {
                                                        case "inSheet": frame.InSheet = int.Parse(frameAttr.Value); break;
                                                        case "inAnim": frame.InAnim = int.Parse(frameAttr.Value); break;
                                                        case "animType": frame.AnimType = frameAttr.Value; break;
                                                        case "onFraction": frame.OnFraction = double.Parse(frameAttr.Value); break;
                                                        case "offFraction": frame.OffFraction = double.Parse(frameAttr.Value); break;
                                                        case "singleFrame": frame.SingleFrame = ReadContentAsStringAsBoolean(frameAttr.Value); break;
                                                        default: Log.Debug($"Unknown frame attribute: '{frameAttrName}'."); break;
                                                    }
                                                }
                                                enemy.Frames.Add(frame);
                                                break;
                                            }
                                        case "shadow":
                                            {
                                                var shadowEl = enemyElEl;
                                                var shadow = enemy.Shadow;
                                                shadow.Path = shadowEl.Value;
                                                foreach (var shadowAttr in shadowEl.Attributes())
                                                {
                                                    var shadowAttrName = shadowAttr.Name.ToString();
                                                    switch (shadowAttrName)
                                                    {
                                                        case "xOff": shadow.OffsetX = int.Parse(shadowAttr.Value); break;
                                                        case "yOff": shadow.OffsetY = int.Parse(shadowAttr.Value); break;
                                                        default: Log.Debug($"Unknown shadow attribute: '{shadowAttrName}'."); break;
                                                    }
                                                }
                                                break;
                                            }
                                        case "particle":
                                            {
                                                var particleEl = enemyElEl;
                                                var particle = enemy.Particle;
                                                foreach (var particleAttr in particleEl.Attributes())
                                                {
                                                    var particleAttrName = particleAttr.Name.ToString();
                                                    switch (particleAttrName)
                                                    {
                                                        case "hit": particle.HitPath = particleAttr.Value; break;
                                                        default: Log.Debug($"Unknown particle attribute: '{particleAttrName}'."); break;
                                                    }
                                                }
                                                break;
                                            }
                                        case "tweens":
                                            {
                                                var tweensEl = enemyElEl;
                                                var tweens = enemy.Tweens;
                                                foreach (var tweensAttr in tweensEl.Attributes())
                                                {
                                                    var tweensAttrName = tweensAttr.Name.ToString();
                                                    switch (tweensAttrName)
                                                    {
                                                        case "move": tweens.Move = tweensAttr.Value; break;
                                                        case "moveShadow": tweens.MoveShadow = tweensAttr.Value; break;
                                                        case "hit": tweens.Hit = tweensAttr.Value; break;
                                                        case "hitShadow": tweens.HitShadow = tweensAttr.Value; break;
                                                        default: Log.Debug($"Unknown tweens attribute: '{tweensAttrName}'."); break;
                                                    }
                                                }
                                                break;
                                            }
                                        case "optionalStats":
                                            {
                                                var optionalStatsEl = enemyElEl;
                                                var optionalStats = enemy.OptionalStats;
                                                foreach (var optionalStatsAttr in optionalStatsEl.Attributes())
                                                {
                                                    var optionalStatsAttrName = optionalStatsAttr.Name.ToString();
                                                    switch (optionalStatsAttrName)
                                                    {
                                                        case "boss": optionalStats.Boss = ReadContentAsStringAsBoolean(optionalStatsAttr.Value); break;
                                                        case "bounceOnMovementFail": optionalStats.BounceOnMovementFail = ReadContentAsStringAsBoolean(optionalStatsAttr.Value); break;
                                                        case "floating": optionalStats.Floating = ReadContentAsStringAsBoolean(optionalStatsAttr.Value); break;
                                                        case "ignoreLiquids": optionalStats.IgnoreLiquids = ReadContentAsStringAsBoolean(optionalStatsAttr.Value); break;
                                                        case "ignoreWalls": optionalStats.IgnoreWalls = ReadContentAsStringAsBoolean(optionalStatsAttr.Value); break;
                                                        case "isMonkeyLike": optionalStats.IsMonkeyLike = ReadContentAsStringAsBoolean(optionalStatsAttr.Value); break;
                                                        case "massive": optionalStats.Massive = ReadContentAsStringAsBoolean(optionalStatsAttr.Value); break;
                                                        case "miniboss": optionalStats.Miniboss = ReadContentAsStringAsBoolean(optionalStatsAttr.Value); break;
                                                        default: Log.Debug($"Unknown optionalStats attribute: '{optionalStatsAttrName}'."); break;
                                                    }
                                                }
                                                break;
                                            }
                                        case "spritesheet":
                                            {
                                                var spritesheetEl = enemyElEl;
                                                var spriteSheet = enemy.SpriteSheet;
                                                spriteSheet.Path = spritesheetEl.Value;
                                                foreach (var spritesheetAttr in spritesheetEl.Attributes())
                                                {
                                                    var spritesheetAttrName = spritesheetAttr.Name.ToString();
                                                    switch (spritesheetAttrName)
                                                    {
                                                        case "numFrames": spriteSheet.FrameCount = int.Parse(spritesheetAttr.Value); break;
                                                        case "frameW": spriteSheet.FrameWidth = int.Parse(spritesheetAttr.Value); break;
                                                        case "frameH": spriteSheet.FrameHeight = int.Parse(spritesheetAttr.Value); break;
                                                        case "xOff": spriteSheet.OffsetX = int.Parse(spritesheetAttr.Value); break;
                                                        case "yOff": spriteSheet.OffsetY = int.Parse(spritesheetAttr.Value); break;
                                                        case "zOff": spriteSheet.OffsetZ = int.Parse(spritesheetAttr.Value); break;
                                                        case "heartXOff": spriteSheet.HeartOffsetX = int.Parse(spritesheetAttr.Value); break;
                                                        case "heartYOff": spriteSheet.HeartOffsetY = int.Parse(spritesheetAttr.Value); break;
                                                        default: Log.Debug($"Unknown spritesheet attribute: '{spritesheetAttrName}'."); break;
                                                    }
                                                }
                                                break;
                                            }
                                        case "stats":
                                            {
                                                var statsEl = enemyElEl;
                                                var stats = enemy.Stats;
                                                foreach (var statsAttr in statsEl.Attributes())
                                                {
                                                    var statsAttrName = statsAttr.Name.ToString();
                                                    switch (statsAttrName)
                                                    {
                                                        case "beatsPerMove": stats.BeatsPerMove = int.Parse(statsAttr.Value); break;
                                                        case "coinsToDrop": stats.CoinsToDrop = int.Parse(statsAttr.Value); break;
                                                        case "damagePerHit": stats.DamagePerHit = int.Parse(statsAttr.Value); break;
                                                        case "health": stats.Health = int.Parse(statsAttr.Value); break;
                                                        case "movement": stats.Movement = statsAttr.Value; break;
                                                        case "priority": stats.Priority = int.Parse(statsAttr.Value); break;
                                                        default: Log.Debug($"Unknown stats attribute: '{statsAttrName}'."); break;
                                                    }
                                                }
                                                break;
                                            }
                                        default: Log.Debug($"Unknown enemy element: '{enemyElElName}'."); break;
                                    }
                                }

                                enemy.Name = (enemy.FriendlyName ?? enemy.ElementName).ToTitleCase();

                                enemies.Add(enemy);
                            }
                            break;
                        }
                    case "characters":
                        {
                            var charactersEl = rootElEl;
                            var characters = data.Characters;
                            foreach (var characterEl in charactersEl.Elements())
                            {
                                var characterElName = characterEl.Name.ToString();
                                var character = new Character();

                                foreach (var characterAttr in characterEl.Attributes())
                                {
                                    var characterAttrName = characterAttr.Name.ToString();
                                    switch (characterAttrName)
                                    {
                                        case "id": character.Id = int.Parse(characterAttr.Value); break;
                                        default: Log.Debug($"Unknown character attribute: '{characterAttrName}'."); break;
                                    }
                                }

                                foreach (var characterElEl in characterEl.Elements())
                                {
                                    var characterElElName = characterElEl.Name.ToString();
                                    switch (characterElElName)
                                    {
                                        case "initial_equipment":
                                            {
                                                var initialEquipmentEl = characterElEl;
                                                foreach (var initialEquipmentElEl in initialEquipmentEl.Elements())
                                                {
                                                    var initialEquipmentElElName = initialEquipmentElEl.Name.ToString();
                                                    switch (initialEquipmentElElName)
                                                    {
                                                        case "item":
                                                            {
                                                                var itemEl = initialEquipmentElEl;
                                                                foreach (var itemAttr in itemEl.Attributes())
                                                                {
                                                                    var itemAttrName = itemAttr.Name.ToString();
                                                                    switch (itemAttrName)
                                                                    {
                                                                        case "type":
                                                                            {
                                                                                // Items must be parsed already.
                                                                                var item = data.Items.Single(i => i.ElementName == itemAttr.Value);
                                                                                character.InitialEquipment.Add(item);
                                                                                break;
                                                                            }
                                                                        default: Log.Debug($"Unknown item attribute: '{itemAttrName}'."); break;
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        case "cursed":
                                                            {
                                                                var cursedEl = initialEquipmentElEl;
                                                                var cursedSlot = new CursedSlot();
                                                                foreach (var cursedAttr in cursedEl.Attributes())
                                                                {
                                                                    var cursedAttrName = cursedAttr.Name.ToString();
                                                                    switch (cursedAttrName)
                                                                    {
                                                                        case "slot": cursedSlot.Slot = cursedAttr.Value; break;
                                                                        default: Log.Debug($"Unknown cursed attribute: '{cursedAttrName}'."); break;
                                                                    }
                                                                }
                                                                character.CursedSlots.Add(cursedSlot);
                                                                break;
                                                            }
                                                        default: Log.Debug($"Unknown initial_equipment element: '{initialEquipmentElElName}'."); break;
                                                    }
                                                }
                                                break;
                                            }
                                        default: Log.Debug($"Unknown character element: '{characterElElName}'."); break;
                                    }
                                }

                                characters.Add(character);
                            }
                            break;
                        }
                    case "modes":
                        {
                            var modesEl = rootElEl;
                            var modes = data.Modes;
                            foreach (var modeEl in modesEl.Elements())
                            {
                                var modeElName = modeEl.Name.ToString();
                                switch (modeElName)
                                {
                                    case "hard":
                                        {
                                            var hardEl = modeEl;
                                            var hardMode = new HardMode();
                                            foreach (var hardAttr in hardEl.Attributes())
                                            {
                                                var hardAttrName = hardAttr.Name.ToString();
                                                switch (hardAttrName)
                                                {
                                                    case "extraEnemiesPerRoom": hardMode.ExtraEnemiesPerRoom = int.Parse(hardAttr.Value); break;
                                                    case "extraMinibossesPerExit": hardMode.ExtraEnemiesPerRoom = int.Parse(hardAttr.Value); break;
                                                    case "upgradeStairLockingMinibosses": hardMode.UpgradeStairLockingMinibosses = ReadContentAsStringAsBoolean(hardAttr.Value); break;
                                                    case "minibossesPerNonExit": hardMode.MinibossesPerNonExit = int.Parse(hardAttr.Value); break;
                                                    case "disableTrapdoors": hardMode.DisableTrapdoors = ReadContentAsStringAsBoolean(hardAttr.Value); break;
                                                    case "harderBosses": hardMode.HarderBosses = ReadContentAsStringAsBoolean(hardAttr.Value); break;
                                                    case "sarcSpawnTimer": hardMode.SarcSpawnTimer = int.Parse(hardAttr.Value); break;
                                                    case "sarcsPerRoom": hardMode.SarcsPerRoom = int.Parse(hardAttr.Value); break;
                                                    case "spawnHelperItems": hardMode.SpawnHelperItems = ReadContentAsStringAsBoolean(hardAttr.Value); break;
                                                    default: Log.Debug($"Unknown hard attribute: '{hardAttrName}'."); break;
                                                }
                                            }
                                            modes.Add(hardMode);
                                            break;
                                        }
                                    default: Log.Debug($"Unknown mode element: '{modeElName}'."); break;
                                }
                            }
                            break;
                        }
                    default: Log.Debug($"Unknown necrodancer element: '{rootElElName}'."); break;
                }
            }

            return data;
        }

        /// <summary>
        /// A collection of strings that represent <c>true</c>.
        /// </summary>
        static readonly IEnumerable<string> TrueStrings = new[] { "1", "y", "yes", "true" };
        /// <summary>
        /// A collection of strings that represent <c>false</c>.
        /// </summary>
        static readonly IEnumerable<string> FalseStrings = new[] { "0", "n", "no", "false" };

        /// <summary>
        /// Reads the text content at the current position as a <see cref="bool" />.
        /// </summary>
        /// <param name="content">
        /// The text content.
        /// </param>
        /// <returns>The text content as a <see cref="bool" />.</returns>
        /// <exception cref="InvalidCastException">
        /// The value could not be read as a boolean.
        /// </exception>
        /// <remarks>
        /// This method is more relaxed than <see cref="XmlReader.ReadContentAsBoolean" /> and will read values 
        /// that may not be XML-compliant (e.g. "True", "False").
        /// </remarks>
        static bool ReadContentAsStringAsBoolean(string content)
        {
            if (TrueStrings.Contains(content, StringComparer.OrdinalIgnoreCase))
                return true;
            if (FalseStrings.Contains(content, StringComparer.OrdinalIgnoreCase))
                return false;

            throw new InvalidCastException("Only the following are supported for converting strings to boolean: "
                + string.Join(",", TrueStrings)
                + " and "
                + string.Join(",", FalseStrings));
        }

        static string GetName(DisplayString flyaway, string elementName)
        {
            var name = !flyaway.Equals(DisplayString.Empty) ? flyaway.Text : elementName;

            return name.ToTitleCase();
        }
    }
}

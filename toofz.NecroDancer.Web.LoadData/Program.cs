using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.EntityFramework;

namespace toofz.NecroDancer.Web.LoadData
{
    class Program
    {
        /// <summary>
        /// A collection of strings that represent <c>true</c>.
        /// </summary>
        private static readonly IEnumerable<string> TrueStrings = new[] { "1", "y", "yes", "true" };
        /// <summary>
        /// A collection of strings that represent <c>false</c>.
        /// </summary>
        private static readonly IEnumerable<string> FalseStrings = new[] { "0", "n", "no", "false" };

        /// <summary>
        /// Reads the text content at the current position as a <see cref="bool" />.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader" /> to read with.</param>
        /// <returns>The text content as a <see cref="bool" />.</returns>
        /// <exception cref="InvalidCastException">
        /// The value could not be read as a boolean.
        /// </exception>
        /// <remarks>
        /// This method is more relaxed than <see cref="XmlReader.ReadContentAsBoolean" /> and will read values 
        /// that may not be XML-compliant (e.g. "True", "False").
        /// </remarks>
        public static bool ReadContentAsStringAsBoolean(string content)
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

        static void Main(string[] args)
        {
            using (var db = new NecroDancerContext())
            {
                // TODO: This shouldn't be hardcoded.
                var doc = XDocument.Load(@"S:\Applications\Steam\steamapps\common\Crypt of the NecroDancer\data\necrodancer.xml");
                var root = doc.Element("necrodancer");

                foreach (var itemEl in root.Element("items").Elements())
                {
                    var item = new Item { ElementName = itemEl.Name.ToString(), ImagePath = itemEl.Value };

                    foreach (var attr in itemEl.Attributes())
                    {
                        switch (attr.Name.ToString())
                        {
                            case "slot": item.Slot = attr.Value; break;
                            case "diamondCost": item.DiamondCost = int.Parse(attr.Value); break;
                            case "coinCost": item.CoinCost = int.Parse(attr.Value); break;
                            case "flyaway": item.Flyaway = new DisplayString(attr.Value); break;

                            case "isArmor": item.IsArmor = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isBlood": item.IsBlood = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isBlunderbuss": item.IsBlunderbuss = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isBow": item.IsBow = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isBroadsword": item.IsBroadsword = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isCat": item.IsCat = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isCoin": item.IsCoin = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isCrossbow": item.IsCrossbow = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isDagger": item.IsDagger = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isDiamond": item.IsDiamond = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isFlail": item.IsFlail = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isFood": item.IsFood = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isFrost": item.IsFrost = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isGlass": item.IsGlass = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isGold": item.IsGold = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isLongsword": item.IsLongsword = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isObsidian": item.IsObsidian = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isPhasing": item.IsPhasing = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isPiercing": item.IsPiercing = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isRapier": item.IsRapier = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isRifle": item.IsRifle = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isScroll": item.IsScroll = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isShovel": item.IsShovel = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isSpear": item.IsSpear = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isSpell": item.IsSpell = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isTitanium": item.IsTitanium = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isTorch": item.IsTorch = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isWeapon": item.IsWeapon = ReadContentAsStringAsBoolean(attr.Value); break;
                            case "isWhip": item.IsWhip = ReadContentAsStringAsBoolean(attr.Value); break;
                            default: break;
                        }
                    }

                    item.Name = GetName(item.Flyaway, item.ElementName);

                    db.Set<Item>().Add(item);
                }

                foreach (var enemyEl in root.Element("enemies").Elements())
                {
                    var enemy = new Enemy { ElementName = enemyEl.Name.ToString() };

                    foreach (var attr in enemyEl.Attributes())
                    {
                        switch (attr.Name.ToString())
                        {
                            case "type": enemy.Type = int.Parse(attr.Value); break;
                            case "id": enemy.Id = int.Parse(attr.Value); break;
                            case "friendlyName": enemy.FriendlyName = attr.Value; break;
                            default: break;
                        }
                    }

                    foreach (var el in enemyEl.Elements())
                    {
                        switch (el.Name.ToString())
                        {
                            case "spritesheet":
                                enemy.SpriteSheet.Path = el.Value.ToString();
                                break;
                            case "stats":
                                var stats = enemy.Stats;
                                foreach (var attr in el.Attributes())
                                {
                                    switch (attr.Name.ToString())
                                    {
                                        case "beatsPerMove": stats.BeatsPerMove = int.Parse(attr.Value); break;
                                        case "coinsToDrop": stats.CoinsToDrop = int.Parse(attr.Value); break;
                                        case "damagePerHit": stats.DamagePerHit = int.Parse(attr.Value); break;
                                        case "health": stats.Health = int.Parse(attr.Value); break;
                                        case "movement": stats.Movement = attr.Value; break;
                                        case "priority": stats.Priority = int.Parse(attr.Value); break;
                                        default: break;
                                    }
                                }
                                break;
                            case "optionalStats":
                                foreach (var attr in el.Attributes())
                                {
                                    switch (attr.Name.ToString())
                                    {
                                        case "floating":
                                            if (ReadContentAsStringAsBoolean(attr.Value)) { enemy.OptionalStats |= OptionalStats.Floating; }
                                            break;
                                        case "bounceOnMovementFail":
                                            if (ReadContentAsStringAsBoolean(attr.Value)) { enemy.OptionalStats |= OptionalStats.BounceOnMovementFail; }
                                            break;
                                        case "ignoreWalls":
                                            if (ReadContentAsStringAsBoolean(attr.Value)) { enemy.OptionalStats |= OptionalStats.Phasing; }
                                            break;
                                        case "miniboss":
                                            if (ReadContentAsStringAsBoolean(attr.Value)) { enemy.OptionalStats |= OptionalStats.Miniboss; }
                                            break;
                                        case "massive":
                                            if (ReadContentAsStringAsBoolean(attr.Value)) { enemy.OptionalStats |= OptionalStats.Massive; }
                                            break;
                                        case "ignoreLiquids":
                                            if (ReadContentAsStringAsBoolean(attr.Value)) { enemy.OptionalStats |= OptionalStats.IgnoreLiquids; }
                                            break;
                                        case "boss":
                                            if (ReadContentAsStringAsBoolean(attr.Value)) { enemy.OptionalStats |= OptionalStats.Boss; }
                                            break;
                                        default: break;
                                    }
                                }
                                break;
                            default: break;
                        }
                    }

                    enemy.Name = (enemy.FriendlyName ?? enemy.ElementName).ToTitleCase();

                    db.Set<Enemy>().Add(enemy);
                }

                db.SaveChanges();
            }
        }
    }
}

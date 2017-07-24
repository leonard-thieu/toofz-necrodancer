using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using toofz.Xml;

namespace toofz.NecroDancer.Data
{
    [DebuggerDisplay("{Name}")]
    public sealed class Enemy : IXmlSerializable
    {
        public const string XmlArrayName = "enemies";
        private static readonly XmlSerializer SpriteSheetSerializer = new XmlSerializer(typeof(SpriteSheet));
        private static readonly XmlSerializer FrameSerializer = new XmlSerializer(typeof(Frame));
        private static readonly XmlSerializer ShadowSerializer = new XmlSerializer(typeof(Shadow));
        private static readonly XmlSerializer StatsSerializer = new XmlSerializer(typeof(Stats));
        private static readonly XmlSerializer BouncerSerializer = new XmlSerializer(typeof(Bouncer));
        private static readonly XmlSerializer TweensSerializer = new XmlSerializer(typeof(Tweens));
        private static readonly XmlSerializer ParticleSerializer = new XmlSerializer(typeof(Particle));

        public string ElementName { get; set; }
        public int Type { get; set; }

        public int? Id { get; set; }
        public string FriendlyName { get; set; }
        public bool LevelEditor { get; set; } = true;

        public SpriteSheet SpriteSheet { get; set; } = new SpriteSheet();
        public List<Frame> Frames { get; private set; } = new List<Frame>();
        public Shadow Shadow { get; set; } = new Shadow();
        public Stats Stats { get; set; } = new Stats();
        public OptionalStats OptionalStats { get; set; }
        public Bouncer Bouncer { get; set; } = new Bouncer();
        public Tweens Tweens { get; set; } = new Tweens();
        public Particle Particle { get; set; } = new Particle();

        public string Name { get; set; }
        public string ImagePath => SpriteSheet.Path;
        public int FrameCount
        {
            get { return SpriteSheet.FrameCount / 2; }
            set { SpriteSheet.FrameCount = value; }
        }

        #region IXmlSerializable Members

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            ElementName = reader.LocalName;

            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "type": Type = reader.ReadContentAsInt(); break;
                    case "id": Id = reader.ReadContentAsInt(); break;
                    case "friendlyName": FriendlyName = reader.ReadContentAsString(); break;
                    case "levelEditor": LevelEditor = reader.ReadContentAsStringAsBoolean(); break;
                    default:
                        Trace.TraceWarning($"Unknown attribute '{reader.LocalName}'.");
                        break;
                }
            }

            Name = (FriendlyName ?? ElementName).ToTitleCase();

            reader.ReadStartElement(ElementName);

            do
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case "spritesheet":
                                SpriteSheet = SpriteSheetSerializer.Deserialize(reader) as SpriteSheet;
                                break;
                            case "frame":
                                var frame = FrameSerializer.Deserialize(reader) as Frame;
                                Frames.Add(frame);
                                break;
                            case "shadow":
                                Shadow = ShadowSerializer.Deserialize(reader) as Shadow;
                                break;
                            case "stats":
                                Stats = StatsSerializer.Deserialize(reader) as Stats;
                                break;
                            case "optionalStats":
                                var isEmpty = reader.IsEmptyElement;

                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.LocalName)
                                    {
                                        case "floating":
                                            if (reader.ReadContentAsStringAsBoolean()) { OptionalStats |= OptionalStats.Floating; }
                                            break;
                                        case "bounceOnMovementFail":
                                            if (reader.ReadContentAsStringAsBoolean()) { OptionalStats |= OptionalStats.BounceOnMovementFail; }
                                            break;
                                        case "ignoreWalls":
                                            if (reader.ReadContentAsStringAsBoolean()) { OptionalStats |= OptionalStats.Phasing; }
                                            break;
                                        case "miniboss":
                                            if (reader.ReadContentAsStringAsBoolean()) { OptionalStats |= OptionalStats.Miniboss; }
                                            break;
                                        case "massive":
                                            if (reader.ReadContentAsStringAsBoolean()) { OptionalStats |= OptionalStats.Massive; }
                                            break;
                                        case "ignoreLiquids":
                                            if (reader.ReadContentAsStringAsBoolean()) { OptionalStats |= OptionalStats.IgnoreLiquids; }
                                            break;
                                        case "boss":
                                            if (reader.ReadContentAsStringAsBoolean()) { OptionalStats |= OptionalStats.Boss; }
                                            break;
                                        default:
                                            Trace.TraceWarning($"Unknown attribute '{reader.LocalName}'.");
                                            break;
                                    }
                                }

                                reader.ReadStartElement("optionalStats");

                                if (isEmpty)
                                    break;

                                reader.FindAndReadEndElement();
                                break;
                            case "bouncer":
                                Bouncer = BouncerSerializer.Deserialize(reader) as Bouncer;
                                break;
                            case "tweens":
                                Tweens = TweensSerializer.Deserialize(reader) as Tweens;
                                break;
                            case "particle":
                                Particle = ParticleSerializer.Deserialize(reader) as Particle;
                                break;
                            default:
                                Trace.TraceWarning($"Unknown element '{reader.LocalName}'.");
                                break;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        reader.ReadEndElement();
                        return;

                    default:
                        if (!reader.Read())
                        {
                            return;
                        }
                        break;
                }
            } while (true);
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using toofz.Xml;

namespace toofz.NecroDancer.Dungeons
{
    [XmlRoot(XmlName)]
    public sealed class Level : INotifyPropertyChanged, IXmlSerializable
    {
        #region Static Members

        #region XML Names

        public const string XmlName = "level";

        const string BossXmlName = "bossNum";
        const string MusicXmlName = "music";
        const string IdXmlName = "num";

        #endregion

        #region Serializers

        static readonly XmlSerializer TileSerializer = new XmlSerializer(typeof(Tile));
        static readonly XmlSerializer TrapSerializer = new XmlSerializer(typeof(Trap));
        static readonly XmlSerializer EnemySerializer = new XmlSerializer(typeof(Enemy));
        static readonly XmlSerializer ItemSerializer = new XmlSerializer(typeof(Item));
        static readonly XmlSerializer ChestSerializer = new XmlSerializer(typeof(Chest));
        static readonly XmlSerializer CrateSerializer = new XmlSerializer(typeof(Crate));
        static readonly XmlSerializer ShrineSerializer = new XmlSerializer(typeof(Shrine));

        #endregion

        #endregion

        #region Properties

        public int Id { get; set; }
        public int Boss { get; set; } = -1;
        public int Music { get; set; }
        public EntityQuadtree Entities { get; } = new EntityQuadtree { Bounds = new RectangleF(-200, -200, 400, 400) };

        #endregion

#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        #region IXmlSerializable Members

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case BossXmlName: Boss = reader.ReadContentAsInt(); break;
                    case MusicXmlName: Music = reader.ReadContentAsInt(); break;
                    case IdXmlName: Id = reader.ReadContentAsInt(); break;
                    default:
                        var lineInfo = "";
                        var xli = reader as IXmlLineInfo;
                        if (xli != null)
                        {
                            lineInfo = $" at line {xli.LineNumber}, position {xli.LinePosition}";
                        }
                        Trace.TraceWarning($"Unknown attribute '{reader.LocalName}' while reading element '{XmlName}'{lineInfo}.");
                        break;
                }
            }

            reader.ReadStartElement("level");
            reader.ReadNestedSequence<Tile, Entity>("tiles", Entities, TileSerializer);
            reader.ReadNestedSequence<Trap, Entity>("traps", Entities, TrapSerializer);
            reader.ReadNestedSequence<Enemy, Entity>("enemies", Entities, EnemySerializer, p => ((Enemy)p).Boss = Boss);
            reader.ReadNestedSequence<Item, Entity>("items", Entities, ItemSerializer);
            reader.ReadNestedSequence<Chest, Entity>("chests", Entities, ChestSerializer);
            reader.ReadNestedSequence<Crate, Entity>("crates", Entities, CrateSerializer);
            reader.ReadNestedSequence<Shrine, Entity>("shrines", Entities, ShrineSerializer);
            reader.FindAndReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttribute(BossXmlName, Boss);
            writer.WriteAttribute(MusicXmlName, Music);
            writer.WriteAttribute(IdXmlName, Id);

            writer.WriteNestedSequence<Tile, Entity>(Entities, "tiles", TileSerializer);
            writer.WriteNestedSequence<Trap, Entity>(Entities, "traps", TrapSerializer);
            writer.WriteNestedSequence<Enemy, Entity>(Entities, "enemies", EnemySerializer);
            writer.WriteNestedSequence<Item, Entity>(Entities, "items", ItemSerializer);
            writer.WriteNestedSequence<Chest, Entity>(Entities, "chests", ChestSerializer);
            writer.WriteNestedSequence<Crate, Entity>(Entities, "crates", CrateSerializer);
            writer.WriteNestedSequence<Shrine, Entity>(Entities, "shrines", ShrineSerializer);
        }

        #endregion
    }
}

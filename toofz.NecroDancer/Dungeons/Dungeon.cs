using System.ComponentModel;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FluentValidation;
using toofz.Xml;

namespace toofz.NecroDancer.Dungeons
{
    [XmlRoot(XmlName)]
    public sealed class Dungeon : INotifyPropertyChanged, IXmlSerializable
    {
        #region Static Members

        const string XmlName = "dungeon";

        const string CharacterXmlName = "character";
        const string NameXmlName = "name";
        const string NumLevelsXmlName = "numLevels";

        static readonly XmlSerializer LevelSerializer = new XmlSerializer(typeof(Level));

        #endregion

        public int Character { get; set; } = -1;
        public string Name { get; set; } = "MY DUNGEON";
        public LevelCollection Levels { get; } = new LevelCollection();

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
                    case CharacterXmlName: Character = reader.ReadContentAsInt(); break;
                    case NameXmlName: Name = reader.ReadContentAsString(); break;
                    case NumLevelsXmlName: /* Ignore */ break;

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

            reader.ReadStartElement(XmlName);
            reader.ReadSequence(Levels, LevelSerializer);
            reader.FindAndReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartAttribute(CharacterXmlName);
            writer.WriteValue(Character);
            writer.WriteEndAttribute();

            writer.WriteStartAttribute(NameXmlName);
            writer.WriteValue(Name);
            writer.WriteEndAttribute();

            writer.WriteStartAttribute(NumLevelsXmlName);
            writer.WriteValue(Levels.Count);
            writer.WriteEndAttribute();

            foreach (var level in Levels)
            {
                writer.WriteStartElement(Level.XmlName);
                level.WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        #endregion
    }

    sealed class DungeonValidator : AbstractValidator<Dungeon>
    {
        public DungeonValidator()
        {
            RuleFor(x => x.Character).Must(c =>
            {
                switch (c)
                {
                    case -1:
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 1000:
                    case 1001:
                    case 1002:
                    case 1003:
                    case 1004:
                    case 1005:
                    case 1006:
                    case 1007:
                    case 1008:
                    case 1009: return true;
                    default: return false;
                }
            });
            RuleFor(x => x.Name).Length(1, 10).Matches(@"[\w\s]{1,10}");
        }
    }
}

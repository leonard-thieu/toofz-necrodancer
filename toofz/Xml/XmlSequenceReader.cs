using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;

namespace toofz.Xml
{
    public sealed class XmlSequenceReader<TBase>
    {
        #region Initialization

        public XmlSequenceReader(XmlReader reader, ICollection<TBase> collection)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            isIXmlSerializable = typeof(TBase).Implements<IXmlSerializable>();
            sequenceMaps = new FuncKeyedCollection<string, XmlSequenceMapping<TBase>>(k => k.ElementName);

            this.reader = reader;
            this.collection = collection;
        }

        #endregion

        #region Fields

        private readonly bool isIXmlSerializable;
        private readonly FuncKeyedCollection<string, XmlSequenceMapping<TBase>> sequenceMaps;

        private readonly XmlReader reader;
        private readonly ICollection<TBase> collection;

        #endregion

        #region Public Methods

        public void AddMapping<T>(XmlSerializer serializer)
            where T : TBase
        {
            AddMapping<T>(serializer, null);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void AddMapping<T>(XmlSerializer serializer, Action<TBase> onItemCreated)
            where T : TBase
        {
            var mapping = new XmlSequenceMapping<TBase>(typeof(T), serializer, onItemCreated);
            sequenceMaps.Add(mapping);
        }

        public void ReadNested(string startElementName)
        {
            if (startElementName == null)
                throw new ArgumentNullException(nameof(startElementName));

            reader.ReadStartElement(startElementName);
            Read();
            reader.FindAndReadEndElement();
        }

        public void Read()
        {
            do
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        var mapping = GetMapping();

                        if (mapping != null)
                        {
                            ReadSequence(mapping);
                        }
                        else
                        {
                            Trace.TraceWarning($"Unknown element '{reader.LocalName}'.");
                        }
                        break;

                    case XmlNodeType.EndElement:
                        return;

                    default:
                        if (!reader.Read())
                        {
                            throw new InvalidOperationException("Unexpected end of file.");
                        }
                        break;
                }
            } while (true);
        }

        #endregion

        #region Private Methods

        private XmlSequenceMapping<TBase> GetMapping()
        {
            XmlSequenceMapping<TBase> value;

            if (!sequenceMaps.TryGetValue(reader.LocalName, out value))
            {
                sequenceMaps.TryGetValue(XmlSequenceMapping<TBase>.AnyElementName, out value);
            }

            return value;
        }

        private void ReadSequence(XmlSequenceMapping<TBase> mapping)
        {
            if (mapping == null)
                throw new ArgumentNullException(nameof(mapping));

            do
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        TBase item;

                        if (isIXmlSerializable)
                        {
                            item = Activator.CreateInstance<TBase>();
                            ((IXmlSerializable)item).ReadXml(reader);
                        }
                        else
                        {
                            item = (TBase)mapping.Serializer.Deserialize(reader);
                        }

                        if (mapping.OnItemCreated != null)
                        {
                            mapping.OnItemCreated(item);
                        }

                        collection.Add(item);
                        break;

                    case XmlNodeType.EndElement:
                        return;

                    default:
                        if (!reader.Read())
                        {
                            throw new InvalidOperationException("Unexpected end of file.");
                        }
                        break;
                }
            } while (true);
        }

        #endregion
    }
}

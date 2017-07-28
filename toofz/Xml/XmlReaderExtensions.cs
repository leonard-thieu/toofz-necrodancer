using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace toofz.Xml
{
    /// <summary>
    /// Contains extension methods for <see cref="XmlReader" />.
    /// </summary>
    public static class XmlReaderExtensions
    {
        /// <summary>
        /// Advances the reader until it passes an <see cref="XmlNodeType.EndElement" /> or there are no more 
        /// nodes to read.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader" /> to read with.</param>
        public static void FindAndReadEndElement(this XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            do
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.EndElement:
                        reader.ReadEndElement();
                        return;
                }
            } while (reader.Read());
        }

        #region ReadSequence

        public static void ReadSequence<TBase>(this XmlReader reader, ICollection<TBase> collection, XmlSerializer serializer)
        {
            ReadSequence<TBase, TBase>(reader, collection, serializer);
        }

        public static void ReadSequence<T, TBase>(this XmlReader reader, ICollection<TBase> collection, XmlSerializer serializer)
            where T : TBase
        {
            ReadSequence<T, TBase>(reader, collection, serializer, null);
        }

        public static void ReadSequence<T, TBase>(this XmlReader reader, ICollection<TBase> collection, XmlSerializer serializer, Action<TBase> onItemCreated)
            where T : TBase
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            var sequenceReader = new XmlSequenceReader<TBase>(reader, collection);
            sequenceReader.AddMapping<T>(serializer, onItemCreated);
            sequenceReader.Read();
        }

        #endregion

        #region ReadNestedSequence

        public static void ReadNestedSequence<TBase>(this XmlReader reader, string startElementName, ICollection<TBase> collection, XmlSerializer serializer)
        {
            ReadNestedSequence<TBase, TBase>(reader, startElementName, collection, serializer);
        }

        public static void ReadNestedSequence<T, TBase>(this XmlReader reader, string startElementName, ICollection<TBase> collection, XmlSerializer serializer)
            where T : TBase
        {
            ReadNestedSequence<T, TBase>(reader, startElementName, collection, serializer, null);
        }

        public static void ReadNestedSequence<T, TBase>(this XmlReader reader, string startElementName, ICollection<TBase> collection, XmlSerializer serializer, Action<TBase> onItemCreated)
            where T : TBase
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (startElementName == null)
                throw new ArgumentNullException(nameof(startElementName));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            var isEmpty = reader.IsEmptyElement;

            reader.ReadStartElement(startElementName);

            if (isEmpty)
                return;

            ReadSequence<T, TBase>(reader, collection, serializer, onItemCreated);
            reader.FindAndReadEndElement();
        }

        #endregion
    }
}

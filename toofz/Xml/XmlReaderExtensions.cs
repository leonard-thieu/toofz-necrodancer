using System;
using System.Collections.Generic;
using System.Linq;
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
        public static bool ReadContentAsStringAsBoolean(this XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var content = reader.ReadContentAsString();

            if (TrueStrings.Contains(content, StringComparer.OrdinalIgnoreCase))
                return true;
            if (FalseStrings.Contains(content, StringComparer.OrdinalIgnoreCase))
                return false;

            throw new InvalidCastException("Only the following are supported for converting strings to boolean: "
                + string.Join(",", TrueStrings)
                + " and "
                + string.Join(",", FalseStrings));
        }

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
    }
}

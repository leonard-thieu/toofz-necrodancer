using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace toofz.Xml
{
    /// <summary>
    /// Contains extension methods for <see cref="XmlWriter" />.
    /// </summary>
    public static class XmlWriterExtensions
    {
        /// <summary>
        /// If <paramref name="value" /> is not equal to the default value of <typeparamref name="T" />, writes an attribute 
        /// using the specified name and value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="writer">The <see cref="XmlWriter" /> to write with.</param>
        /// <param name="localName">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer" /> is null.
        /// </exception>
        public static void WriteAttribute<T>(this XmlWriter writer, string localName, T value)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (string.IsNullOrEmpty(localName))
                throw new ArgumentException();

            WriteAttribute<T>(writer, localName, value, default(T));
        }

        /// <summary>
        /// If <paramref name="value" /> is not equal to <paramref name="defaultValue" />, writes an attribute 
        /// using the specified name and value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="writer">The <see cref="XmlWriter" /> to write with.</param>
        /// <param name="localName">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <param name="defaultValue">
        /// The default value of the attribute. If <paramref name="value" /> is equal to this parameter, nothing will be written.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer" /> is null.
        /// </exception>
        public static void WriteAttribute<T>(this XmlWriter writer, string localName, T value, T defaultValue)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (string.IsNullOrEmpty(localName))
                throw new ArgumentException();

            if (value != null && !EqualityComparer<T>.Default.Equals(value, defaultValue))
            {
                writer.WriteAttributeString(localName, value.ToString());
            }
        }

        public static void WriteNestedSequence<T, TBase>(this XmlWriter writer, ICollection<TBase> collection, string startElementName, XmlSerializer serializer)
            where T : TBase
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (string.IsNullOrEmpty(startElementName))
                throw new ArgumentException();

            var sequenceWriter = new XmlSequenceWriter<TBase>(writer, collection);

            writer.WriteStartElement(startElementName);
            sequenceWriter.Write<T>(serializer);
            writer.WriteEndElement();
        }
    }
}

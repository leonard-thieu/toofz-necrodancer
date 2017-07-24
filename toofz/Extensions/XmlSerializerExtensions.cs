using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace toofz.Xml
{
    /// <summary>
    /// Contains extenions for <see cref="XmlSerializer" />.
    /// </summary>
    public static class XmlSerializerExtensions
    {
        public static T Load<T>(this XmlSerializer serializer, string path)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException();

            using (var xpr = new XmlPreprocessingReader(path))
            {
                return Load<T>(serializer, xpr.BaseStream);
            }
        }

        public static T Parse<T>(this XmlSerializer serializer, string xml)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));

            using (var sr = xml.ToStream())
            {
                var xpr = new XmlPreprocessingReader(sr);

                return Load<T>(serializer, xpr.BaseStream);
            }
        }

        public static T Load<T>(this XmlSerializer xs, Stream stream)
        {
            if (xs == null)
                throw new ArgumentNullException(nameof(xs));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var xr = XmlReaderFactory.Create(stream))
            {
                return (T)xs.Deserialize(xr);
            }
        }

        private static XmlSerializerNamespaces GetBlankNamespace()
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            return ns;
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o", Justification = "Matches Microsoft API.")]
        public static void SerializeWithoutNamespaces(this XmlSerializer serializer, XmlWriter xmlWriter, object o)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            serializer.Serialize(xmlWriter, o, GetBlankNamespace());
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o", Justification = "Matches Microsoft API.")]
        public static void SerializeWithoutNamespaces(this XmlSerializer serializer, TextWriter textWriter, object o)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            serializer.Serialize(textWriter, o, GetBlankNamespace());
        }
    }
}

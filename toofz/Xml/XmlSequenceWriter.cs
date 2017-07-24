using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace toofz.Xml
{
    public sealed class XmlSequenceWriter<TBase>
    {
        public XmlSequenceWriter(XmlWriter writer, ICollection<TBase> collection)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            this.writer = writer;
            this.collection = collection;
        }

        private readonly XmlWriter writer;
        private readonly ICollection<TBase> collection;

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void Write<T>(XmlSerializer serializer)
            where T : TBase
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            foreach (var item in collection.OfType<T>())
            {
                serializer.SerializeWithoutNamespaces(writer, item);
            }
        }
    }
}

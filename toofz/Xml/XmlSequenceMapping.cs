using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace toofz.Xml
{
    public sealed class XmlSequenceMapping<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public const string AnyElementName = "*";

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSequenceMapping{T}" /> class.
        /// </summary>
        /// <param name="type">The type of the XML items.</param>
        /// <param name="serializer">The serializer to use to deserialize the XML items.</param>
        /// <param name="onItemCreated">The delegate to call after an item has been created. This may be null.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializer" /> is null.
        /// </exception>
        public XmlSequenceMapping(Type type, XmlSerializer serializer, Action<T> onItemCreated)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            string elementName;
            Util.TryGetAttributeProperty(type, (XmlRootAttribute a) => a.ElementName, out elementName);

            ElementName = elementName ?? AnyElementName;
            Serializer = serializer;
            OnItemCreated = onItemCreated;
        }

        #endregion

        #region Properties

        public string ElementName { get; private set; }
        /// <summary>
        /// The serializer to use to deserialize the XML array items.
        /// </summary>
        public XmlSerializer Serializer { get; private set; }
        /// <summary>
        /// The delegate to call after an item has been created. This is passed the newly created item.
        /// </summary>
        public Action<T> OnItemCreated { get; private set; }

        #endregion
    }
}

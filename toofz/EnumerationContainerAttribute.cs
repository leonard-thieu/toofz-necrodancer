using System;

namespace toofz
{
    /// <summary>
    /// Indicates the type to search for instances of the attributed class.
    /// </summary>
    /// <remarks>
    /// Instances should be marked as public and static.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class EnumerationContainerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationContainerAttribute" /> class.
        /// </summary>
        /// <param name="containerType">The type that contains instances of the attributed class.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="containerType" /> is null.
        /// </exception>
        public EnumerationContainerAttribute(Type containerType)
        {
            if (containerType == null)
                throw new ArgumentNullException(nameof(containerType));

            ContainerType = containerType;
        }

        /// <summary>
        /// The type that contains instances of the attributed class.
        /// </summary>
        public Type ContainerType { get; }
    }
}

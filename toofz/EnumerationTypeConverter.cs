using System;
using System.ComponentModel;
using System.Globalization;

namespace toofz
{
    /// <summary>
    /// Converts a <see cref="Enumeration" /> to and from a <see cref="string" />.
    /// </summary>
    /// <typeparam name="T">A type that derives from <see cref="Enumeration" />.</typeparam>
    public sealed class EnumerationTypeConverter<T> : TypeConverter
        where T : Enumeration
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to <see cref="Enumeration" />.
        /// </summary>
        /// <param name="context">
        /// An <see cref="ITypeDescriptorContext" /> that provides a format context. This parameter is not used.
        /// </param>
        /// <param name="sourceType">A <see cref="Type" /> that represents the type you want to convert from.</param>
        /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts a <see cref="string" /> to <typeparamref name="T" />.
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var name = value as string;
            if (name != null)
            {
                return Enumeration.Parse<T>(name, true);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value != null)
            {
                return value.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

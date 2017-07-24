using System;
using System.Xml;

namespace toofz.NecroDancer.Data
{
    internal static class XmlReaderExtensions
    {
        public static DisplayString ReadContentAsDisplayString(this XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            return new DisplayString(reader.ReadContentAsString());
        }
    }
}

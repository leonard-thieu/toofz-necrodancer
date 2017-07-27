using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;

namespace toofz.Xml
{
    public static class XmlReaderFactory
    {
        private static readonly XmlReaderSettings Settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
            DtdProcessing = DtdProcessing.Ignore
        };

        public static XmlReader Create(Stream input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return XmlReader.Create(input, Settings);
        }
    }
}

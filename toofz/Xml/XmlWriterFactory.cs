using System;
using System.IO;
using System.Xml;

namespace toofz.Xml
{
    public static class XmlWriterFactory
    {
        private static readonly XmlWriterSettings Settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "    ",
            OmitXmlDeclaration = true
        };

        public static XmlWriter Create(string outputFileName)
        {
            if (outputFileName == null)
                throw new ArgumentNullException(nameof(outputFileName));

            return XmlWriter.Create(outputFileName, Settings);
        }

        public static XmlWriter Create(TextWriter output)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            return XmlWriter.Create(output, Settings);
        }
    }
}

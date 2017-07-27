﻿using System;
using System.IO;

namespace toofz.Xml
{
    public sealed class XmlPreprocessingReader : StreamReader
    {
        internal const string InvalidXmlDeclaration = "<?xml?>";

        public XmlPreprocessingReader(string path)
            : base(path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException();

            Initialize();
        }

        public XmlPreprocessingReader(Stream input)
            : base(input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            Initialize();
        }

        private void Initialize()
        {
            SkipInvalidXmlDeclaration();
        }

        private void SkipInvalidXmlDeclaration()
        {
            var position = (int)BaseStream.Position;

            var declLength = InvalidXmlDeclaration.Length;
            var remainingLength = BaseStream.Length - BaseStream.Position;

            if (remainingLength >= declLength)
            {
                var buffer = new char[declLength];
                ReadBlock(buffer, position, declLength);

                var decl = new string(buffer);
                BaseStream.Position = decl != InvalidXmlDeclaration ? position : position + declLength;
            }
        }
    }
}

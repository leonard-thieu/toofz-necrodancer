using System;
using System.IO;

namespace toofz.NecroDancer.Replays
{
    public sealed class ReplaySerializer
    {
        public ReplayData Deserialize(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var reader = new ReplaySerializationReader(stream);

            return reader.Deserialize();
        }

        public void Serialize(Stream stream, ReplayData replay)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var writer = new ReplaySerializationWriter(stream);
            writer.Serialize(replay);
        }
    }
}

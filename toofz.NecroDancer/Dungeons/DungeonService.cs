using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using toofz.Xml;

namespace toofz.NecroDancer.Dungeons
{
    public static class DungeonService
    {
        static readonly XmlSerializer DungeonSerializer = new XmlSerializer(typeof(Dungeon));

        public static async Task<Dungeon> LoadAsync(string path)
        {
            using (var ms = new MemoryStream())
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    await fs.CopyToAsync(ms).ConfigureAwait(false);
                }
                ms.Position = 0;

                using (var xpr = new XmlPreprocessingReader(ms))
                using (var xr = XmlReaderFactory.Create(xpr.BaseStream))
                {
                    return (Dungeon)DungeonSerializer.Deserialize(xr);
                }
            }
        }

        public static async Task SaveAsync(Dungeon dungeon, string path)
        {
            using (var ms = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(ms))
                {
                    DungeonSerializer.Serialize(xw, dungeon);
                }
                ms.Position = 0;

                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, true))
                {
                    await ms.CopyToAsync(fs).ConfigureAwait(false);
                }
            }
        }
    }
}

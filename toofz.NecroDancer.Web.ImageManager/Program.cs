using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using toofz.NecroDancer.Data;

namespace toofz.NecroDancer.Web.ImageManager
{
    class ImageFile
    {
        public ImageFile(string baseName, int frameIndex, string type, string ext, Guid format, byte[] data)
        {
            FrameIndex = frameIndex;
            Format = format;
            Data = data;

            Name = $"{baseName}_{frameIndex}_{type}{ext}";
        }

        public int FrameIndex { get; }
        public Guid Format { get; }
        public byte[] Data { get; }
        public string Name { get; }
    }

    internal class Program
    {
        // TODO: This shouldn't be hardcoded.
        private const string DataDirectory = @"S:\Applications\Steam\steamapps\common\Crypt of the NecroDancer\data";

        private static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 100;

            var storageConnectionString = Util.GetEnvVar("toofzStorageConnectionString");
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("crypt");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            RunAsync(container).Wait();
        }

        private static Task RunAsync(CloudBlobContainer container)
        {
            var path = Path.Combine(DataDirectory, "necrodancer.xml");
            var doc = XDocument.Load(path);
            var root = doc.Element("necrodancer");

            var tasks = new List<Task>();

            var itemImages = new List<ImageFile>();
            foreach (var itemEl in root.Element("items").Elements())
            {
                var item = new Item { ElementName = itemEl.Name.ToString(), ImagePath = Path.Combine("items", itemEl.Value) };

                foreach (var attr in itemEl.Attributes())
                {
                    switch (attr.Name.ToString())
                    {
                        case "numFrames": item.FrameCount = int.Parse(attr.Value); break;
                        default: break;
                    }
                }

                var frames = GetImageFrames(item, "items");
                itemImages.AddRange(frames);
            }
            var itemTasks = from i in itemImages
                            select UploadImageAsync(container, i);
            tasks.AddRange(itemTasks);

            var enemyImages = new List<ImageFile>();
            foreach (var enemyEl in root.Element("enemies").Elements())
            {
                var enemy = new Enemy { ElementName = enemyEl.Name.ToString() };

                foreach (var attr in enemyEl.Attributes())
                {
                    switch (attr.Name.ToString())
                    {
                        case "type": enemy.Type = int.Parse(attr.Value); break;
                        default: break;
                    }
                }

                foreach (var el in enemyEl.Elements())
                {
                    switch (el.Name.ToString())
                    {
                        case "spritesheet":
                            enemy.SpriteSheet.Path = el.Value.ToString();
                            foreach (var attr in el.Attributes())
                            {
                                switch (attr.Name.ToString())
                                {
                                    case "numFrames": enemy.SpriteSheet.FrameCount = int.Parse(attr.Value); break;
                                    default: break;
                                }
                            }
                            break;
                        case "frame":
                            var frame = new Frame();
                            foreach (var attr in el.Attributes())
                            {
                                switch (attr.Name.ToString())
                                {
                                    case "inSheet": frame.InSheet = int.Parse(attr.Value); break;
                                    case "inAnim": frame.InAnim = int.Parse(attr.Value); break;
                                    case "animType": frame.AnimType = attr.Value; break;
                                    case "onFraction": frame.OnFraction = double.Parse(attr.Value); break;
                                    case "offFraction": frame.OffFraction = double.Parse(attr.Value); break;
                                    case "singleFrame": frame.SingleFrame = attr.Value; break;
                                    default: break;
                                }
                            }
                            enemy.Frames.Add(frame);
                            break;
                    }
                }

                var frames = GetImageFrames(enemy, "enemies");
                enemyImages.AddRange(frames);
            }
            var enemyTasks = from e in enemyImages
                             select UploadImageAsync(container, e);
            tasks.AddRange(enemyTasks);

            return Task.WhenAll(tasks);
        }

        private static IEnumerable<ImageFile> GetImageFrames(Item item, string directory)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (item.ImagePath == null)
                throw new ArgumentNullException(nameof(item.ImagePath));
            if (item.ElementName == null)
                throw new ArgumentNullException(nameof(item.ElementName));
            if (item.FrameCount < 1)
                throw new ArgumentException();
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            var frameCount = item.FrameCount;
            var path = Path.Combine(DataDirectory, item.ImagePath);
            var baseName = Path.Combine(directory, item.ElementName);

            return GetImages(baseName, frameCount, path);
        }

        private static IEnumerable<ImageFile> GetImageFrames(Enemy enemy, string directory)
        {
            if (enemy == null)
                throw new ArgumentNullException(nameof(enemy));
            if (enemy.ImagePath == null)
                throw new ArgumentNullException(nameof(enemy.ImagePath));
            if (enemy.FrameCount < 1)
                throw new ArgumentException();
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            var frameCount = enemy.FrameCount;
            var path = Path.Combine(DataDirectory, enemy.ImagePath);
            var baseName = Path.Combine(directory, enemy.ElementName + enemy.Type);

            return GetImages(baseName, frameCount, path);
        }

        private static IEnumerable<ImageFile> GetImages(string baseName, int frameCount, string path)
        {
            if (frameCount < 1)
                throw new ArgumentException();
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            using (var image = Image.FromFile(path))
            {
                // Items always have 2 rows of sprites. Each sprite is equally sized.
                //   - Top:    Normal
                //   - Bottom: Shadow
                var width = image.Width / frameCount;
                var height = image.Height / 2;
                var destRect = new Rectangle(0, 0, width, height);
                var extension = Path.GetExtension(path);

                return GetImages(baseName, frameCount, image, width, height, extension);
            }
        }

        private static IEnumerable<ImageFile> GetImages(string baseName, int frameCount, Image image, int width, int height, string extension)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            int srcX;
            int srcY;
            var imageFiles = new List<ImageFile>();

            for (int y = 0; y < 2; y++)
            {
                srcY = y * height;

                for (int x = 0; x < frameCount; x++)
                {
                    srcX = x * width;
                    var i = (y * frameCount) + x;

                    var data = GetSpriteData(image, srcX, srcY, width, height);
                    var defaultImage = new ImageFile(baseName, i, "d", extension, image.RawFormat.Guid, data);
                    imageFiles.Add(defaultImage);
                    var smallimage = ResizeImageSmall(baseName, defaultImage, extension);
                    imageFiles.Add(smallimage);
                    var mediumImage = ResizeImageMedium(baseName, defaultImage, extension);
                    imageFiles.Add(mediumImage);
                    var largeImage = ResizeImageLarge(baseName, defaultImage, extension);
                    imageFiles.Add(largeImage);
                }
            }

            return imageFiles;
        }

        private static byte[] GetSpriteData(Image image, int srcX, int srcY, int width, int height)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            using (var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {

                using (var g = Graphics.FromImage(bitmap))
                using (var ms = new MemoryStream())
                {
                    var br = new BinaryReader(ms);

                    var destRect = new Rectangle(0, 0, width, height);
                    g.DrawImage(image, destRect, srcX, srcY, width, height, GraphicsUnit.Pixel);
                    bitmap.Save(ms, image.RawFormat);
                    ms.Position = 0;

                    return br.ReadBytes((int)ms.Length);
                }
            }
        }

        private static ImageFile ResizeImageSmall(string sourceFileName, ImageFile image, string extension)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var data = ResizeImage(image.Data, 24, 24, 1);

            return new ImageFile(sourceFileName, image.FrameIndex, "s", extension, image.Format, data);
        }

        private static ImageFile ResizeImageMedium(string sourceFileName, ImageFile image, string extension)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var data = ResizeImage(image.Data, 36, 36, 1);

            return new ImageFile(sourceFileName, image.FrameIndex, "m", extension, image.Format, data);
        }

        private static ImageFile ResizeImageLarge(string sourceFileName, ImageFile image, string extension)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var data = ResizeImage(image.Data, 56, 56, float.MaxValue);

            return new ImageFile(sourceFileName, image.FrameIndex, "l", extension, image.Format, data);
        }

        private static byte[] ResizeImage(byte[] data, int width, int height, float minScale = 1)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            using (var msIn = new MemoryStream())
            {
                var bw = new BinaryWriter(msIn);
                bw.Write(data);

                using (var image = Image.FromStream(msIn))
                {
                    var pageUnit = GraphicsUnit.Pixel;
                    var srcRect = image.GetBounds(ref pageUnit);

                    var scaleX = (float)width / image.Width;
                    var scaleY = (float)height / image.Height;
                    var scale = new[] { scaleX, scaleY, minScale }.Min();
                    scale = Math.Max(scale, float.Epsilon);

                    var destWidth = image.Width * scale;
                    var destHeight = image.Height * scale;
                    var destX = (width - destWidth) / 2;
                    var destY = (height - destHeight) / 2;
                    var destRect = new RectangleF(destX, destY, destWidth, destHeight);

                    using (var bitmap = new Bitmap(width, height))
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.DrawImage(image, destRect, srcRect, pageUnit);

                        using (var msOut = new MemoryStream())
                        {
                            var br = new BinaryReader(msOut);
                            bitmap.Save(msOut, image.RawFormat);
                            msOut.Position = 0;

                            return br.ReadBytes((int)msOut.Length);
                        }
                    }
                }
            }
        }

        private static async Task UploadImageAsync(CloudBlobContainer container, ImageFile image)
        {
            var blockBlob = container.GetBlockBlobReference(image.Name);
            blockBlob.Properties.ContentType = "image/png";
            blockBlob.Properties.CacheControl = "max-age=604800";
            await blockBlob.UploadFromByteArrayAsync(image.Data, 0, image.Data.Length);
            Console.WriteLine($"Uploaded {image.Name}.");
        }
    }
}

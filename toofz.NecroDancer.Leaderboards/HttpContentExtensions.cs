using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    static class HttpContentExtensions
    {
        /// <summary>
        /// Copies the content to a seekable stream and reports progress.
        /// </summary>
        /// <param name="content">The content to copy.</param>
        /// <param name="progress">The progress object to update. This may be null if progress reporting is not desired.</param>
        /// <returns>A task that represents copying of the content.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="content"/> is null.
        /// </exception>
        public static async Task<Stream> ProcessContentAsync(this HttpContent content, IProgress<long> progress)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content), $"{nameof(content)} is null.");

            var contentStream = new MemoryStream();
            await content.CopyToAsync(contentStream).ConfigureAwait(false);
            contentStream.Position = 0;

            // Get the compressed size
            progress?.Report(contentStream.Length);

            if (content.Headers.ContentEncoding.Contains("gzip"))
            {
                using (var gzip = new GZipStream(contentStream, CompressionMode.Decompress))
                {
                    // GZipStream is not seekable
                    var decompressed = new MemoryStream();
                    await gzip.CopyToAsync(decompressed).ConfigureAwait(false);
                    decompressed.Position = 0;
                    return decompressed;
                }
            }
            else
            {
                return contentStream;
            }
        }
    }
}

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using toofz.NecroDancer.Replays;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Contains extension methods for <see cref="ICloudBlob"/>.
    /// </summary>
    internal static class ICloudBlobExtensions
    {
        private static readonly ReplaySerializer ReplaySerializer = new ReplaySerializer();

        /// <summary>
        /// Uploads a locally readable replay to Azure.
        /// </summary>
        /// <param name="blob">The blob to upload the replay to.</param>
        /// <param name="replayData">The replay to upload.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for a task to complete.</param>
        /// <returns>
        /// A <see cref="Task{Uri}"/> object that represents the asynchronus operation. The object contains the URI of the replay.
        /// </returns>
        public static async Task<Uri> UploadReplayDataAsync(this ICloudBlob blob, ReplayData replayData, CancellationToken cancellationToken)
        {
            if (blob == null)
                throw new ArgumentNullException(nameof(blob));
            if (replayData == null)
                throw new ArgumentNullException(nameof(replayData));

            using (var ms = new MemoryStream())
            {
                ReplaySerializer.Serialize(ms, replayData);
                ms.Position = 0;

                await blob.UploadFromStreamAsync(ms, cancellationToken).ConfigureAwait(false);

                return blob.Uri;
            }
        }

        /// <summary>
        /// Uploads a remote replay to Azure.
        /// </summary>
        /// <param name="blob">The blob to upload the replay to.</param>
        /// <param name="replayData">The replay to upload.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for a task to complete.</param>
        /// <returns>
        /// A <see cref="Task{Uri}"/> object that represents the asynchronus operation. The object contains the URI of the replay.
        /// </returns>
        public static async Task<Uri> UploadRemoteReplayDataAsync(this ICloudBlob blob, Stream replayData, CancellationToken cancellationToken)
        {
            if (blob == null)
                throw new ArgumentNullException(nameof(blob));
            if (replayData == null)
                throw new ArgumentNullException(nameof(replayData));

            using (var ms = new MemoryStream())
            {
                await replayData.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;

                await blob.UploadFromStreamAsync(ms, cancellationToken).ConfigureAwait(false);

                return blob.Uri;
            }
        }
    }
}

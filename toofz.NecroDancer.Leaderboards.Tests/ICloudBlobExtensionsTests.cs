using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using toofz.NecroDancer.Replays;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ICloudBlobExtensionsTests
    {
        [TestClass]
        public class UploadReplayDataAsync
        {
            [TestMethod]
            public async Task ReturnsUri()
            {
                // Arrange
                var mockBlob = new Mock<ICloudBlob>();
                mockBlob.Setup(x => x.UploadFromStreamAsync(null, CancellationToken.None));
                mockBlob.Setup(x => x.Uri).Returns(Constants.FakeUri);

                var replay = new ReplayData { Header = new Replays.Header() };

                // Act
                var ex = await Record.ExceptionAsync(() => ICloudBlobExtensions.UploadReplayDataAsync(mockBlob.Object, replay, CancellationToken.None));

                // Assert
                Assert.IsNull(ex);
            }

            [TestMethod]
            public async Task NullBlob_ReturnsArgumentNullException()
            {
                // Arrange
                ICloudBlob blob = null;

                var replay = new ReplayData();

                // Act
                var ex = await Record.ExceptionAsync(() => ICloudBlobExtensions.UploadReplayDataAsync(blob, replay, CancellationToken.None));

                // Assert
                Assert.IsNotNull(ex);
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task NullReplay_ReturnsArgumentNullException()
            {
                // Arrange
                var mockBlob = new Mock<ICloudBlob>();
                mockBlob.Setup(x => x.UploadFromStreamAsync(null, CancellationToken.None));
                mockBlob.Setup(x => x.Uri).Returns(Constants.FakeUri);

                ReplayData replay = null;

                // Act
                var ex = await Record.ExceptionAsync(() => ICloudBlobExtensions.UploadReplayDataAsync(mockBlob.Object, replay, CancellationToken.None));

                // Assert
                Assert.IsNotNull(ex);
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }
    }
}

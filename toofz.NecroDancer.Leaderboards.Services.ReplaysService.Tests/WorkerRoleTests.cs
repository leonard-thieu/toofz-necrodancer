using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using toofz.NecroDancer.Leaderboards.toofz;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Services.ReplaysService.Tests
{
    class WorkerRoleTests
    {
        [TestClass]
        public class UpdateReplaysAsync
        {
            static CloudBlobDirectory GetReplaysCloudBlobDirectory()
            {
                var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("crypt");
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                return container.GetDirectoryReference("replays");
            }

            [TestMethod]
            public async Task ApiClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockISteamWebApiClient = new Mock<ISteamWebApiClient>();
                var mockIUgcHttpClient = new Mock<IUgcHttpClient>();
                var directory = GetReplaysCloudBlobDirectory();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateReplaysAsync(
                        null,
                        mockISteamWebApiClient.Object,
                        mockIUgcHttpClient.Object,
                        directory,
                        1);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task SteamWebApiClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockIToofzApiClient = new Mock<IToofzApiClient>();
                var mockIUgcHttpClient = new Mock<IUgcHttpClient>();
                var directory = GetReplaysCloudBlobDirectory();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateReplaysAsync(
                        mockIToofzApiClient.Object,
                        null,
                        mockIUgcHttpClient.Object,
                        directory,
                        1);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task UgcHttpClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockIToofzApiClient = new Mock<IToofzApiClient>();
                var mockISteamWebApiClient = new Mock<ISteamWebApiClient>();
                var directory = GetReplaysCloudBlobDirectory();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateReplaysAsync(
                        mockIToofzApiClient.Object,
                        mockISteamWebApiClient.Object,
                        null,
                        directory,
                        1);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task DirectoryIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockIToofzApiClient = new Mock<IToofzApiClient>();
                var mockISteamWebApiClient = new Mock<ISteamWebApiClient>();
                var mockIUgcHttpClient = new Mock<IUgcHttpClient>();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateReplaysAsync(
                        mockIToofzApiClient.Object,
                        mockISteamWebApiClient.Object,
                        mockIUgcHttpClient.Object,
                        null,
                        1);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task NegativeLimit_ThrowsArgumentOutOfRangeException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockIToofzApiClient = new Mock<IToofzApiClient>();
                var mockISteamWebApiClient = new Mock<ISteamWebApiClient>();
                var mockIUgcHttpClient = new Mock<IUgcHttpClient>();
                var directory = GetReplaysCloudBlobDirectory();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateReplaysAsync(
                        mockIToofzApiClient.Object,
                        mockISteamWebApiClient.Object,
                        mockIUgcHttpClient.Object,
                        directory,
                        -1);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentOutOfRangeException));
            }

            [TestMethod]
            public async Task ValidParams_UpdatesReplays()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockIToofzApiClient = new Mock<IToofzApiClient>();
                mockIToofzApiClient
                    .Setup(toofzApiClient => toofzApiClient.GetReplaysAsync(It.IsAny<GetReplaysParams>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new Replays()));
                mockIToofzApiClient
                    .Setup(toofzApiClient => toofzApiClient.PostReplaysAsync(It.IsAny<IEnumerable<Replay>>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new BulkStore()));

                var mockISteamWebApiClient = new Mock<ISteamWebApiClient>();
                var mockIUgcHttpClient = new Mock<IUgcHttpClient>();
                var directory = GetReplaysCloudBlobDirectory();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateReplaysAsync(
                        mockIToofzApiClient.Object,
                        mockISteamWebApiClient.Object,
                        mockIUgcHttpClient.Object,
                        directory,
                        1);
                });

                // Assert
                Assert.IsNull(ex, ex?.Message);
            }
        }
    }
}

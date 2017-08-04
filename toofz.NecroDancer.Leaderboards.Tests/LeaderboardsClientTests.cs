using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class LeaderboardsClientTests
    {
        [TestClass]
        public class UpdatePlayersAsync
        {
            [TestMethod]
            public async Task NegativeLimit_ThrowsArgumentOutOfRangeException()
            {
                // Arrange
                var mockSteamWebApiClient = new Mock<ISteamWebApiClient>();
                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();
                var mockApiClient = new Mock<IApiClient>();
                var mockUgcHttpClient = new Mock<IUgcHttpClient>();
                var leaderboardsClient = new LeaderboardsClient(
                    mockSteamWebApiClient.Object,
                    mockILeaderboardsSqlClient.Object,
                    mockApiClient.Object,
                    mockUgcHttpClient.Object);

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return leaderboardsClient.UpdatePlayersAsync(-1);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentOutOfRangeException));
            }

            [TestMethod]
            public async Task ValidParams_UpdatesPlayers()
            {
                // Arrange
                var mockSteamWebApiClient = new Mock<ISteamWebApiClient>();
                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();
                var mockApiClient = new Mock<IApiClient>();
                var mockUgcHttpClient = new Mock<IUgcHttpClient>();
                var leaderboardsClient = new LeaderboardsClient(
                    mockSteamWebApiClient.Object,
                    mockILeaderboardsSqlClient.Object,
                    mockApiClient.Object,
                    mockUgcHttpClient.Object);

                // Act
                await leaderboardsClient.UpdatePlayersAsync(1);

                // Assert
                mockApiClient.Verify(apiClient => apiClient.PostPlayersAsync(It.IsAny<IEnumerable<Player>>(), It.IsAny<CancellationToken>()));
            }
        }

        [TestClass]
        public class UpdateReplaysAsync
        {
            static CloudBlobDirectory GetDirectory()
            {
                var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("crypt");
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                return container.GetDirectoryReference("replays");
            }

            private static bool _wasUp;

            [ClassInitialize]
            public static void StartAzureBeforeAllTestsIfNotUp(TestContext context)
            {
                if (!AzureStorageEmulatorManager.IsProcessStarted())
                {
                    AzureStorageEmulatorManager.StartStorageEmulator();
                    _wasUp = false;
                }
                else
                {
                    _wasUp = true;
                }

            }

            [ClassCleanup]
            public static void StopAzureAfterAllTestsIfWasDown()
            {
                if (!_wasUp)
                {
                    AzureStorageEmulatorManager.StopStorageEmulator();
                }
                else
                {
                    // Leave as it was before testing...
                }
            }

            [TestMethod]
            public async Task NegativeLimit_ThrowsArgumentOutOfRangeException()
            {
                // Arrange
                var mockSteamWebApiClient = new Mock<ISteamWebApiClient>();
                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();
                var mockApiClient = new Mock<IApiClient>();
                var mockUgcHttpClient = new Mock<IUgcHttpClient>();
                var leaderboardsClient = new LeaderboardsClient(
                    mockSteamWebApiClient.Object,
                    mockILeaderboardsSqlClient.Object,
                    mockApiClient.Object,
                    mockUgcHttpClient.Object);

                var directory = GetDirectory();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return leaderboardsClient.UpdateReplaysAsync(-1, directory);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentOutOfRangeException));
            }

            [TestMethod]
            public async Task DirectoryIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var mockSteamWebApiClient = new Mock<ISteamWebApiClient>();
                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();
                var mockApiClient = new Mock<IApiClient>();
                var mockUgcHttpClient = new Mock<IUgcHttpClient>();
                var leaderboardsClient = new LeaderboardsClient(
                    mockSteamWebApiClient.Object,
                    mockILeaderboardsSqlClient.Object,
                    mockApiClient.Object,
                    mockUgcHttpClient.Object);

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return leaderboardsClient.UpdateReplaysAsync(1, null);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task ValidParams_UpdatesReplays()
            {
                // Arrange
                var mockSteamWebApiClient = new Mock<ISteamWebApiClient>();
                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();
                var mockApiClient = new Mock<IApiClient>();
                var mockUgcHttpClient = new Mock<IUgcHttpClient>();
                var leaderboardsClient = new LeaderboardsClient(
                    mockSteamWebApiClient.Object,
                    mockILeaderboardsSqlClient.Object,
                    mockApiClient.Object,
                    mockUgcHttpClient.Object);

                var directory = GetDirectory();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return leaderboardsClient.UpdateReplaysAsync(1, directory);
                });

                // Assert
                Assert.IsNull(ex);
            }
        }
    }
}

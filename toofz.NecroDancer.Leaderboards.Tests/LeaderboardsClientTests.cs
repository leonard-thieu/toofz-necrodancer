using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using toofz.TestsShared;
using static SteamKit2.SteamUserStats.LeaderboardEntriesCallback;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class LeaderboardsClientTests
    {
        [TestClass]
        public class UpdateLeaderboardsAsync
        {
            [TestMethod]
            public async Task ValidParams_UpdatesLeaderboards()
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

                var mockILeaderboardEntriesCallback = new Mock<ILeaderboardEntriesCallback>();
                mockILeaderboardEntriesCallback
                    .Setup(leaderboardEntries => leaderboardEntries.Entries)
                    .Returns(new ReadOnlyCollection<LeaderboardEntry>(new List<LeaderboardEntry>()));

                var mockISteamClientApiClient = new Mock<ISteamClientApiClient>();
                mockISteamClientApiClient
                    .Setup(steamClient => steamClient.GetLeaderboardEntriesAsync(It.IsAny<uint>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(mockILeaderboardEntriesCallback.Object));

                // Act
                await leaderboardsClient.UpdateLeaderboardsAsync(mockISteamClientApiClient.Object);

                // Assert
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<Leaderboard>>(), It.IsAny<CancellationToken>()));
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<Player>>(), false, It.IsAny<CancellationToken>()));
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<Replay>>(), false, It.IsAny<CancellationToken>()));
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<Entry>>()));
            }
        }

        [TestClass]
        public class UpdateDailyLeaderboardsAsync
        {
            [TestMethod]
            public async Task ValidParams_UpdatesDailyLeaderboards()
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

                var mockIFindOrCreateLeaderboardCallback = new Mock<IFindOrCreateLeaderboardCallback>();

                var mockILeaderboardEntriesCallback = new Mock<ILeaderboardEntriesCallback>();
                mockILeaderboardEntriesCallback
                    .Setup(leaderboardEntries => leaderboardEntries.Entries)
                    .Returns(new ReadOnlyCollection<LeaderboardEntry>(new List<LeaderboardEntry>()));

                var mockISteamClientApiClient = new Mock<ISteamClientApiClient>();
                mockISteamClientApiClient
                    .Setup(steamClient => steamClient.FindLeaderboardAsync(It.IsAny<uint>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(mockIFindOrCreateLeaderboardCallback.Object));
                mockISteamClientApiClient
                    .Setup(steamClient => steamClient.GetLeaderboardEntriesAsync(It.IsAny<uint>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(mockILeaderboardEntriesCallback.Object));

                var dailyLeaderboards = new List<DailyLeaderboard>
                {
                    new DailyLeaderboard
                    {
                        LeaderboardId = 2089328,
                        Date = new DateTime(2017, 8, 6, 0, 0, 0, DateTimeKind.Utc),
                        LastUpdate = new DateTime(2017, 8, 6, 13, 20, 32, DateTimeKind.Utc),
                        ProductId = 1,
                        IsProduction = true,
                    },
                };
                var mockSetDailyLeaderboard = MockHelper.MockSet(dailyLeaderboards);

                var mockLeaderboardsContext = new Mock<LeaderboardsContext>();
                mockLeaderboardsContext
                    .Setup(x => x.DailyLeaderboards)
                    .Returns(mockSetDailyLeaderboard.Object);

                // Act
                await leaderboardsClient.UpdateDailyLeaderboardsAsync(mockISteamClientApiClient.Object, mockLeaderboardsContext.Object);

                // Assert
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<DailyLeaderboard>>(), It.IsAny<CancellationToken>()));
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<Player>>(), false, It.IsAny<CancellationToken>()));
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<Replay>>(), false, It.IsAny<CancellationToken>()));
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<DailyEntry>>(), It.IsAny<CancellationToken>()));
            }
        }

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

                var directory = GetReplaysCloudBlobDirectory();

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

                var directory = GetReplaysCloudBlobDirectory();

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

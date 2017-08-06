using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using toofz.TestsShared;
using static SteamKit2.SteamUserStats.LeaderboardEntriesCallback;

namespace toofz.NecroDancer.Leaderboards.Services.LeaderboardUpdate.Tests
{
    class WorkerRoleTests
    {
        [TestClass]
        public class UpdateLeaderboardsAsync
        {
            [TestMethod]
            public async Task SteamClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateLeaderboardsAsync(
                        null,
                        mockILeaderboardsSqlClient.Object);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task LeaderboardsSqlClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockISteamClientApiClient = new Mock<ISteamClientApiClient>();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateLeaderboardsAsync(
                        mockISteamClientApiClient.Object,
                        null);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task ValidParams_UpdatesLeaderboards()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockILeaderboardEntriesCallback = new Mock<ILeaderboardEntriesCallback>();
                mockILeaderboardEntriesCallback
                    .Setup(leaderboardEntries => leaderboardEntries.Entries)
                    .Returns(new ReadOnlyCollection<LeaderboardEntry>(new List<LeaderboardEntry>()));

                var mockISteamClientApiClient = new Mock<ISteamClientApiClient>();
                mockISteamClientApiClient
                    .Setup(steamClient => steamClient.GetLeaderboardEntriesAsync(It.IsAny<uint>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(mockILeaderboardEntriesCallback.Object));

                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();

                // Act
                await workerRole.UpdateLeaderboardsAsync(
                    mockISteamClientApiClient.Object,
                    mockILeaderboardsSqlClient.Object);

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
            public async Task SteamClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();
                var mockLeaderboardsContext = new Mock<LeaderboardsContext>();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateDailyLeaderboardsAsync(
                        null,
                        mockILeaderboardsSqlClient.Object,
                        mockLeaderboardsContext.Object);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task LeaderboardsSqlClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockISteamClientApiClient = new Mock<ISteamClientApiClient>();
                var mockLeaderboardsContext = new Mock<LeaderboardsContext>();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateDailyLeaderboardsAsync(
                        mockISteamClientApiClient.Object,
                        null,
                        mockLeaderboardsContext.Object);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task DbIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();
                var mockISteamClientApiClient = new Mock<ISteamClientApiClient>();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdateDailyLeaderboardsAsync(
                        mockISteamClientApiClient.Object,
                        mockILeaderboardsSqlClient.Object,
                        null);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task ValidParams_UpdatesDailyLeaderboards()
            {
                // Arrange
                var workerRole = new WorkerRole();

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

                var mockILeaderboardsSqlClient = new Mock<ILeaderboardsSqlClient>();

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
                await workerRole.UpdateDailyLeaderboardsAsync(
                    mockISteamClientApiClient.Object,
                    mockILeaderboardsSqlClient.Object,
                    mockLeaderboardsContext.Object);

                // Assert
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<DailyLeaderboard>>(), It.IsAny<CancellationToken>()));
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<Player>>(), false, It.IsAny<CancellationToken>()));
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<Replay>>(), false, It.IsAny<CancellationToken>()));
                mockILeaderboardsSqlClient.Verify(sqlClient => sqlClient.SaveChangesAsync(It.IsAny<IEnumerable<DailyEntry>>(), It.IsAny<CancellationToken>()));
            }
        }
    }
}

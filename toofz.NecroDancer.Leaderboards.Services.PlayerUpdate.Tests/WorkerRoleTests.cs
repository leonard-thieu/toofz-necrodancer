using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Services.PlayerUpdate.Tests
{
    class WorkerRoleTests
    {
        [TestClass]
        public class UpdatePlayersAsync
        {
            [TestMethod]
            public async Task ApiClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockSteamWebApiClient = new Mock<ISteamWebApiClient>();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdatePlayersAsync(
                        null,
                        mockSteamWebApiClient.Object,
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

                var mockApiClient = new Mock<IApiClient>();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdatePlayersAsync(
                        mockApiClient.Object,
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

                var mockApiClient = new Mock<IApiClient>();
                var mockSteamWebApiClient = new Mock<ISteamWebApiClient>();

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return workerRole.UpdatePlayersAsync(
                        mockApiClient.Object,
                        mockSteamWebApiClient.Object,
                        -1);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentOutOfRangeException));
            }

            [TestMethod]
            public async Task ValidParams_UpdatesPlayers()
            {
                // Arrange
                var workerRole = new WorkerRole();

                var mockApiClient = new Mock<IApiClient>();
                var mockSteamWebApiClient = new Mock<ISteamWebApiClient>();

                // Act
                await workerRole.UpdatePlayersAsync(
                        mockApiClient.Object,
                        mockSteamWebApiClient.Object,
                        1);

                // Assert
                mockApiClient.Verify(apiClient => apiClient.PostPlayersAsync(It.IsAny<IEnumerable<Player>>(), It.IsAny<CancellationToken>()));
            }
        }
    }
}

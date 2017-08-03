using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards.SteamWebApi;

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
                var leaderboardsClient = new LeaderboardsClient(mockSteamWebApiClient.Object, mockILeaderboardsSqlClient.Object, mockApiClient.Object);

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
                var leaderboardsClient = new LeaderboardsClient(mockSteamWebApiClient.Object, mockILeaderboardsSqlClient.Object, mockApiClient.Object);

                // Act
                await leaderboardsClient.UpdatePlayersAsync(1);

                // Assert
                mockApiClient.Verify(apiClient => apiClient.PostPlayersAsync(It.IsAny<IEnumerable<Player>>(), It.IsAny<CancellationToken>()));
            }
        }
    }
}

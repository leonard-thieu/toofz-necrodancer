using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    class SteamClientApiClientTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void UsernameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string userName = null;
                string password = "password";

                // Act
                var ex = Record.Exception(() =>
                {
                    new SteamClientApiClient(userName, password);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public void PasswordIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string userName = "userName";
                string password = null;

                // Act
                var ex = Record.Exception(() =>
                {
                    new SteamClientApiClient(userName, password);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public void ValidParams_ReturnsSteamClientApiClient()
            {
                // Arrange
                string userName = "userName";
                string password = "password";

                // Act
                var client = new SteamClientApiClient(userName, password);

                // Assert
                Assert.IsInstanceOfType(client, typeof(SteamClientApiClient));
            }
        }

        [TestClass]
        public class FindLeaderboardAsync
        {
            const string UserName = "userName";
            const string Password = "password";
            const uint AppId = 247080;
            const string LeaderboardName = "Leaderboard Name";

            Mock<IFindOrCreateLeaderboardCallback> mockIFindOrCreateLeaderboardCallback;
            Mock<ISteamUserStats> mockISteamUserStats;
            Mock<ISteamClient> mockISteamClient;
            Mock<ICallbackManager> mockICallbackManager;
            SteamClientApiClient steamClientApiClient;

            [TestInitialize]
            public void TestInitialize()
            {
                mockIFindOrCreateLeaderboardCallback = new Mock<IFindOrCreateLeaderboardCallback>();

                mockISteamUserStats = new Mock<ISteamUserStats>();
                mockISteamUserStats
                    .Setup(s => s.FindLeaderboard(It.IsAny<uint>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(mockIFindOrCreateLeaderboardCallback.Object));

                mockISteamClient = new Mock<ISteamClient>();
                mockISteamClient
                    .Setup(c => c.GetSteamUserStats())
                    .Returns(mockISteamUserStats.Object);

                mockICallbackManager = new Mock<ICallbackManager>();
                mockICallbackManager
                    .SetupGet(manager => manager.SteamClient)
                    .Returns(mockISteamClient.Object);

                steamClientApiClient = new SteamClientApiClient(UserName, Password, mockICallbackManager.Object);
            }

            [TestMethod]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockIFindOrCreateLeaderboardCallback
                    .Setup(le => le.Result)
                    .Returns(EResult.Fail);

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(SteamClientApiException));
                var e = (SteamClientApiException)ex;
                Assert.AreEqual(EResult.Fail, e.Result);
            }

            [TestMethod]
            public async Task ResultIsOK_ReturnsLeaderboardEntriesCallback()
            {
                // Arrange
                mockIFindOrCreateLeaderboardCallback
                    .Setup(le => le.Result)
                    .Returns(EResult.OK);

                // Act
                var leaderboardEntries = await steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);

                // Assert
                Assert.IsInstanceOfType(leaderboardEntries, typeof(IFindOrCreateLeaderboardCallback));
            }
        }

        [TestClass]
        public class GetLeaderboardEntriesAsync
        {
            const string UserName = "userName";
            const string Password = "password";
            const uint AppId = 247080;
            const int LeaderboardId = 739999;

            Mock<ILeaderboardEntriesCallback> mockILeaderboardEntriesCallback;
            Mock<ISteamUserStats> mockISteamUserStats;
            Mock<ISteamClient> mockISteamClient;
            Mock<ICallbackManager> mockICallbackManager;
            SteamClientApiClient steamClientApiClient;

            [TestInitialize]
            public void TestInitialize()
            {
                mockILeaderboardEntriesCallback = new Mock<ILeaderboardEntriesCallback>();

                mockISteamUserStats = new Mock<ISteamUserStats>();
                mockISteamUserStats
                    .Setup(s => s.GetLeaderboardEntries(It.IsAny<uint>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ELeaderboardDataRequest>()))
                    .Returns(Task.FromResult(mockILeaderboardEntriesCallback.Object));

                mockISteamClient = new Mock<ISteamClient>();
                mockISteamClient
                    .Setup(c => c.GetSteamUserStats())
                    .Returns(mockISteamUserStats.Object);

                mockICallbackManager = new Mock<ICallbackManager>();
                mockICallbackManager
                    .SetupGet(manager => manager.SteamClient)
                    .Returns(mockISteamClient.Object);

                steamClientApiClient = new SteamClientApiClient(UserName, Password, mockICallbackManager.Object);
            }

            [TestMethod]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockILeaderboardEntriesCallback
                    .Setup(le => le.Result)
                    .Returns(EResult.Fail);

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(SteamClientApiException));
                var e = (SteamClientApiException)ex;
                Assert.AreEqual(EResult.Fail, e.Result);
            }

            [TestMethod]
            public async Task ResultIsOK_ReturnsLeaderboardEntriesCallback()
            {
                // Arrange
                mockILeaderboardEntriesCallback
                    .Setup(le => le.Result)
                    .Returns(EResult.OK);

                // Act
                var leaderboardEntries = await steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);

                // Assert
                Assert.IsInstanceOfType(leaderboardEntries, typeof(ILeaderboardEntriesCallback));
            }
        }
    }
}

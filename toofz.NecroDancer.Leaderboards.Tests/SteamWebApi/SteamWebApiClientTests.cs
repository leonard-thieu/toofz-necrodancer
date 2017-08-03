using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.SteamWebApi;
using toofz.NecroDancer.Leaderboards.SteamWebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.SteamWebApi
{
    class SteamWebApiClientTests
    {
        [TestClass]
        public class GetPlayerSummariesAsync
        {
            [TestMethod]
            public async Task SteamWebApiKeyIsNull_ThrowsInvalidOperationException()
            {
                // Arrange
                var steamWebApiClient = new SteamWebApiClient(new MockHttpMessageHandler());

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return steamWebApiClient.GetPlayerSummariesAsync(new long[0]);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }

            [TestMethod]
            public async Task steamIdsIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var steamWebApiClient = new SteamWebApiClient(new MockHttpMessageHandler());
                steamWebApiClient.SteamWebApiKey = "mySteamWebApiKey";

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return steamWebApiClient.GetPlayerSummariesAsync(null);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task TooManySteamIds_ThrowsArgumentException()
            {
                // Arrange
                var steamWebApiClient = new SteamWebApiClient(new MockHttpMessageHandler());
                steamWebApiClient.SteamWebApiKey = "mySteamWebApiKey";

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return steamWebApiClient.GetPlayerSummariesAsync(new long[SteamWebApiClient.MaxPlayerSummariesPerRequest + 1]);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }

            [TestMethod]
            public async Task ValidParams_ReturnsPlayerSummaries()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=mySteamWebApiKey&steamids=76561197960435530")
                    .RespondJson(Resources.GetPlayerSummaries);

                var steamWebApiClient = new SteamWebApiClient(handler);
                steamWebApiClient.SteamWebApiKey = "mySteamWebApiKey";

                // Act
                var playerSummaries = await steamWebApiClient.GetPlayerSummariesAsync(new long[] { 76561197960435530 });

                // Assert
                Assert.IsInstanceOfType(playerSummaries, typeof(PlayerSummaries));
            }
        }
    }
}

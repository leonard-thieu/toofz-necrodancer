using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ApiClientTests
    {
        static readonly CancellationToken Cancellation = CancellationToken.None;

        [TestClass]
        public class GetStaleSteamIdsAsync
        {
            [TestMethod]
            public async Task ReturnsSteamIds()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(new Uri(Constants.FakeUri + "players?limit=0"))
                    .RespondJson(new List<long>());

                var apiClient = new ApiClient(handler)
                {
                    BaseAddress = Constants.FakeUri
                };

                // Act
                var steamIds = await apiClient.GetStaleSteamIdsAsync(0, Cancellation);

                // Assert
                Assert.IsInstanceOfType(steamIds, typeof(IEnumerable<long>));
            }
        }

        [TestClass]
        public class PostPlayersAsync
        {
            [TestMethod]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(new Uri(Constants.FakeUri + "players"))
                    .RespondJson(new { rowsAffected = 1 });

                var apiClient = new ApiClient(handler)
                {
                    BaseAddress = Constants.FakeUri
                };
                var players = new List<Player> { new Player { Exists = true, LastUpdate = new DateTime(2016, 1, 1) } };

                // Act
                var response = await apiClient.PostPlayersAsync(players, Cancellation);
                var rowsAffected = JObject.Parse(response)["rowsAffected"].Value<long>();

                // Assert
                Assert.AreEqual(1, rowsAffected);
            }
        }

        [TestClass]
        public class GetMissingReplayIdsAsync
        {
            [TestMethod]
            public async Task ReturnsReplayIds()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(new Uri(Constants.FakeUri + "replays?limit=0"))
                    .RespondJson(new List<long>());

                var apiClient = new ApiClient(handler)
                {
                    BaseAddress = Constants.FakeUri
                };

                // Act
                var replayIds = await apiClient.GetMissingReplayIdsAsync(0, Cancellation);

                // Assert
                Assert.IsInstanceOfType(replayIds, typeof(IEnumerable<long>));
            }
        }

        [TestClass]
        public class PostReplaysAsync
        {
            [TestMethod]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(new Uri(Constants.FakeUri + "replays"))
                    .RespondJson(new { rowsAffected = 1 });

                var apiClient = new ApiClient(handler)
                {
                    BaseAddress = Constants.FakeUri
                };
                var replays = new List<Replay> { new Replay() };

                // Act
                var response = await apiClient.PostReplaysAsync(replays, Cancellation);
                var rowsAffected = JObject.Parse(response)["rowsAffected"].Value<long>();

                // Assert
                Assert.AreEqual(1, rowsAffected);
            }
        }
    }
}

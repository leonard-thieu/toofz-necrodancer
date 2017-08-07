using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.toofz;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class ToofzApiClientTests
    {
        [TestClass]
        public class GetPlayersAsync
        {
            [TestMethod]
            public async Task ReturnsSteamIds()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(new Uri(Constants.FakeUri + "players?limit=20&sort=updated_at"))
                    .RespondJson(new Players());

                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };

                // Act
                var steamIds = await toofzApiClient.GetPlayersAsync(new GetPlayersParams
                {
                    Limit = 20,
                    Sort = "updated_at",

                });

                // Assert
                Assert.IsInstanceOfType(steamIds, typeof(Players));
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

                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };
                var players = new List<Player> { new Player { Exists = true, LastUpdate = new DateTime(2016, 1, 1) } };

                // Act
                var response = await toofzApiClient.PostPlayersAsync(players);
                var rowsAffected = JObject.Parse(response)["rowsAffected"].Value<long>();

                // Assert
                Assert.AreEqual(1, rowsAffected);
            }
        }

        [TestClass]
        public class GetReplaysAsync
        {
            [TestMethod]
            public async Task ReturnsReplayIds()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(new Uri(Constants.FakeUri + "replays?limit=20"))
                    .RespondJson(new Replays());

                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };

                // Act
                var replayIds = await toofzApiClient.GetReplaysAsync(new GetReplaysParams
                {
                    Limit = 20,
                });

                // Assert
                Assert.IsInstanceOfType(replayIds, typeof(Replays));
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

                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };
                var replays = new List<Replay> { new Replay() };

                // Act
                var response = await toofzApiClient.PostReplaysAsync(replays);
                var rowsAffected = JObject.Parse(response)["rowsAffected"].Value<long>();

                // Assert
                Assert.AreEqual(1, rowsAffected);
            }
        }
    }
}

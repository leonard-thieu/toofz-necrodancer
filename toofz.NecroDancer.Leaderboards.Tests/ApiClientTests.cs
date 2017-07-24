using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ApiClientTests
    {
        private static readonly CancellationToken Cancellation = CancellationToken.None;

        public class GetStaleSteamIdsAsync
        {
            [Fact]
            public async Task ReturnsSteamIds()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.When(new Uri(Constants.FakeUri + "players?limit=0"))
                    .RespondJson(new List<long>());

                var apiClient = new ApiClient(handler);
                apiClient.BaseAddress = Constants.FakeUri;

                // Act
                var steamIds = await apiClient.GetStaleSteamIdsAsync(0, Cancellation);

                // Assert
                Assert.IsAssignableFrom(typeof(IEnumerable<long>), steamIds);
            }
        }

        public class PostPlayersAsync
        {
            [Fact]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.When(new Uri(Constants.FakeUri + "players"))
                    .RespondJson(new { rowsAffected = 1 });

                var apiClient = new ApiClient(handler);
                apiClient.BaseAddress = Constants.FakeUri;

                var players = new List<Player> { new Player { Exists = true, LastUpdate = new DateTime(2016, 1, 1) } };

                // Act
                var response = await apiClient.PostPlayersAsync(players, Cancellation);
                var rowsAffected = JObject.Parse(response)["rowsAffected"].Value<long>();

                // Assert
                Assert.Equal(1, rowsAffected);
            }
        }

        public class GetMissingReplayIdsAsync
        {
            [Fact]
            public async Task ReturnsReplayIds()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.When(new Uri(Constants.FakeUri + "replays?limit=0"))
                    .RespondJson(new List<long>());

                var apiClient = new ApiClient(handler);
                apiClient.BaseAddress = Constants.FakeUri;

                // Act
                var replayIds = await apiClient.GetMissingReplayIdsAsync(0, Cancellation);

                // Assert
                Assert.IsAssignableFrom(typeof(IEnumerable<long>), replayIds);
            }
        }

        public class PostReplaysAsync
        {
            [Fact]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.When(new Uri(Constants.FakeUri + "replays"))
                    .RespondJson(new { rowsAffected = 1 });

                var apiClient = new ApiClient(handler);
                apiClient.BaseAddress = Constants.FakeUri;

                var replays = new List<Replay> { new Replay() };

                // Act
                var response = await apiClient.PostReplaysAsync(replays, Cancellation);
                var rowsAffected = JObject.Parse(response)["rowsAffected"].Value<long>();

                // Assert
                Assert.Equal(1, rowsAffected);
            }
        }
    }
}

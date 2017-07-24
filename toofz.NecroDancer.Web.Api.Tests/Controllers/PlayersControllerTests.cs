using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class PlayersControllerTests
    {
        public class Get_Int64
        {
            [Fact]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSetPlayer = MockHelper.MockSet(new List<Leaderboards.Player>
                {
                    new Leaderboards.Player
                    {
                        SteamId = 0
                    }
                }.AsQueryable());
                var mockSetEntry = MockHelper.MockSet(new List<Leaderboards.Entry>
                {

                }.AsQueryable());
                var mockSetReplay = MockHelper.MockSet(new List<Leaderboards.Replay>
                {

                }.AsQueryable());

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Players).Returns(mockSetPlayer.Object);
                mockRepository.Setup(x => x.Entries).Returns(mockSetEntry.Object);
                mockRepository.Setup(x => x.Replays).Returns(mockSetReplay.Object);

                var mockSqlClient = new Mock<ILeaderboardsSqlClient>();

                var controller = new PlayersController(mockRepository.Object, mockSqlClient.Object, LeaderboardsServiceFactory.Create());

                // Act
                var actionResult = await controller.GetPlayer(0L);
                Assert.IsType<OkNegotiatedContentResult<PlayerEntries>>(actionResult);
                var contentResult = actionResult as OkNegotiatedContentResult<PlayerEntries>;

                // Assert
                Assert.NotNull(contentResult);
                Assert.NotNull(contentResult.Content);
            }
        }

        public class Get_String
        {
            [Fact]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSet = MockHelper.MockSet(new List<Leaderboards.Player>
                {

                }.AsQueryable());

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Players).Returns(mockSet.Object);

                var mockSqlClient = new Mock<ILeaderboardsSqlClient>();

                var controller = new PlayersController(mockRepository.Object, mockSqlClient.Object, LeaderboardsServiceFactory.Create());

                // Act
                var actionResult = await controller.GetPlayers("", new PlayersPagination());
                var contentResult = actionResult as OkNegotiatedContentResult<Players>;

                // Assert
                Assert.NotNull(contentResult);
                Assert.NotNull(contentResult.Content);
            }
        }

        public class Get_Int32
        {
            [Fact]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSet = MockHelper.MockSet(new List<Leaderboards.Player>
                {

                }.AsQueryable());

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Players).Returns(mockSet.Object);

                var mockSqlClient = new Mock<ILeaderboardsSqlClient>();

                var controller = new PlayersController(mockRepository.Object, mockSqlClient.Object, LeaderboardsServiceFactory.Create());

                // Act
                var actionResult = await controller.Get(0);
                var contentResult = actionResult as OkNegotiatedContentResult<List<long>>;

                // Assert
                Assert.NotNull(contentResult);
                Assert.NotNull(contentResult.Content);
            }
        }

        public class Post
        {
            [Fact]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockRepository = new Mock<LeaderboardsContext>();

                var mockSqlClient = new Mock<ILeaderboardsSqlClient>();

                var controller = new PlayersController(mockRepository.Object, mockSqlClient.Object, LeaderboardsServiceFactory.Create());

                // Act
                var actionResult = await controller.Post(new List<PlayerModel>());
                var contentResult = actionResult as OkNegotiatedContentResult<BulkStoreDTO>;

                // Assert
                Assert.NotNull(contentResult);
                Assert.NotNull(contentResult.Content);
            }

            [Fact]
            public async Task InvalidState_ReturnsBadRequest()
            {
                // Arrange
                var mockRepository = new Mock<LeaderboardsContext>();

                var mockSqlClient = new Mock<ILeaderboardsSqlClient>();

                var controller = new PlayersController(mockRepository.Object, mockSqlClient.Object, LeaderboardsServiceFactory.Create());
                controller.ModelState.AddModelError("fakeError", "fakeError");

                // Act
                var actionResult = await controller.Post(new List<PlayerModel>());

                // Assert
                Assert.IsType<InvalidModelStateResult>(actionResult);
            }
        }
    }
}

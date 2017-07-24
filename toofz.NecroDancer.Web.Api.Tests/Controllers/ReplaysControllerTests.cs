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
    public class ReplaysControllerTests
    {
        public class Get
        {
            [Fact]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSet = MockHelper.MockSet(new List<Replay>
                {

                }.AsQueryable());

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Replays).Returns(mockSet.Object);

                var mockSqlClient = new Mock<ILeaderboardsSqlClient>();

                var controller = new ReplaysController(mockRepository.Object, mockSqlClient.Object);

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

                var controller = new ReplaysController(mockRepository.Object, mockSqlClient.Object);

                // Act
                var actionResult = await controller.Post(new List<ReplayModel>());
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

                var controller = new ReplaysController(mockRepository.Object, mockSqlClient.Object);
                controller.ModelState.AddModelError("fakeError", "fakeError");

                // Act
                var actionResult = await controller.Post(new List<ReplayModel>());

                // Assert
                Assert.IsType<InvalidModelStateResult>(actionResult);
            }
        }
    }
}

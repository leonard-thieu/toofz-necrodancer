﻿using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class LeaderboardsControllerTests
    {
        [TestClass]
        public class GetDailies
        {
            [TestMethod]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSetDailyLeaderboard = MockHelper.MockSet<DailyLeaderboard>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.DailyLeaderboards).Returns(mockSetDailyLeaderboard.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    LeaderboardsResources.ReadLeaderboardCategories(),
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var actionResult = await controller.GetDailies(null);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<Models.DailyLeaderboards>));
            }
        }

        [TestClass]
        public class GetLeaderboardEntries
        {
            [TestMethod]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockLeaderboardSet = MockHelper.MockSet(new Leaderboard { LeaderboardId = 741312 });

                var mockEntrySet = MockHelper.MockSet<Entry>();

                var mockReplaySet = MockHelper.MockSet<Replay>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Leaderboards).Returns(mockLeaderboardSet.Object);
                mockRepository.Setup(x => x.Entries).Returns(mockEntrySet.Object);
                mockRepository.Setup(x => x.Replays).Returns(mockReplaySet.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    LeaderboardsResources.ReadLeaderboardCategories(),
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var actionResult = await controller.GetLeaderboardEntries(741312, new Models.LeaderboardsPagination());
                var contentResult = actionResult as OkNegotiatedContentResult<Models.LeaderboardEntries>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsInstanceOfType(contentResult, typeof(OkNegotiatedContentResult<Models.LeaderboardEntries>));
            }

            [TestMethod]
            public async Task ReturnsNotFound()
            {
                // Arrange
                var mockLeaderboardSet = MockHelper.MockSet(new Leaderboard { LeaderboardId = 22 });

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Leaderboards).Returns(mockLeaderboardSet.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    LeaderboardsResources.ReadLeaderboardCategories(),
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var actionResult = await controller.GetLeaderboardEntries(0, new Models.LeaderboardsPagination());

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
            }
        }
    }
}

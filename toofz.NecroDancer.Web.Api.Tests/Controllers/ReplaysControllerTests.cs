﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class ReplaysControllerTests
    {
        [TestClass]
        public class Get
        {
            [TestMethod]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSet = MockHelper.MockSet(new List<Replay>
                {

                }.AsQueryable());

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Replays).Returns(mockSet.Object);

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new ReplaysController(mockRepository.Object, mockILeaderboardsStoreClient.Object);

                // Act
                var actionResult = await controller.Get(0);
                var contentResult = actionResult as OkNegotiatedContentResult<List<long>>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsNotNull(contentResult.Content);
            }
        }

        [TestClass]
        public class Post
        {
            [TestMethod]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockRepository = new Mock<LeaderboardsContext>();

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new ReplaysController(mockRepository.Object, mockILeaderboardsStoreClient.Object);

                // Act
                var actionResult = await controller.Post(new List<ReplayModel>());
                var contentResult = actionResult as OkNegotiatedContentResult<BulkStoreDTO>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsNotNull(contentResult.Content);
            }

            [TestMethod]
            public async Task InvalidState_ReturnsBadRequest()
            {
                // Arrange
                var mockRepository = new Mock<LeaderboardsContext>();

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new ReplaysController(mockRepository.Object, mockILeaderboardsStoreClient.Object);
                controller.ModelState.AddModelError("fakeError", "fakeError");

                // Act
                var actionResult = await controller.Post(new List<ReplayModel>());

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(InvalidModelStateResult));
            }
        }
    }
}

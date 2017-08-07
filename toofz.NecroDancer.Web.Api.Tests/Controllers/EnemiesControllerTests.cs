using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.EntityFramework;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class EnemiesControllerTests
    {
        [TestClass]
        public class GetEnemiesAsync
        {
            [TestMethod]
            public async Task GetEnemiesAsync_ReturnsEnemiesDTO()
            {
                // Arrange
                var mockSet = MockHelper.MockSet<Enemy>();

                var mockRepository = new Mock<NecroDancerContext>();
                mockRepository.Setup(x => x.Enemies).Returns(mockSet.Object);

                var controller = new EnemiesController(mockRepository.Object);

                // Act
                var enemiesDTO = await controller.GetEnemiesAsync(null, new Models.EnemiesPagination(), CancellationToken.None);

                // Assert
                Assert.IsInstanceOfType(enemiesDTO, typeof(Models.Enemies));
            }
        }

        [TestClass]
        public class Get
        {
            [TestMethod]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSet = MockHelper.MockSet<Enemy>();

                var mockRepository = new Mock<NecroDancerContext>();
                mockRepository.Setup(x => x.Enemies).Returns(mockSet.Object);

                var controller = new EnemiesController(mockRepository.Object);

                // Act
                var actionResult = await controller.GetEnemies(new Models.EnemiesPagination());
                var contentResult = actionResult as OkNegotiatedContentResult<Models.Enemies>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsNotNull(contentResult.Content);
            }
        }

        [TestClass]
        public class Get_Attribute
        {
            [TestMethod]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSet = MockHelper.MockSet<Enemy>();

                var mockRepository = new Mock<NecroDancerContext>();
                mockRepository.Setup(x => x.Enemies).Returns(mockSet.Object);

                var controller = new EnemiesController(mockRepository.Object);

                // Act
                var actionResult = await controller.GetEnemies(null, new Models.EnemiesPagination());
                var contentResult = actionResult as OkNegotiatedContentResult<Models.Enemies>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsNotNull(contentResult.Content);
            }
        }
    }
}

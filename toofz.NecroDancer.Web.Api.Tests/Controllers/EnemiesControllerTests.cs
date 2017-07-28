using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.EntityFramework;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class EnemiesControllerTests
    {
        public class GetEnemiesAsync
        {
            [Fact]
            public async Task GetEnemiesAsync_ReturnsEnemiesDTO()
            {
                // Arrange
                var mockSet = MockHelper.MockSet(new List<Data.Enemy>());

                var mockRepository = new Mock<NecroDancerContext>();
                mockRepository.Setup(x => x.Enemies).Returns(mockSet.Object);

                var controller = new EnemiesController(mockRepository.Object);

                // Act
                var enemiesDTO = await controller.GetEnemiesAsync(null, new EnemiesPagination(), CancellationToken.None);

                // Assert
                Assert.IsType<Enemies>(enemiesDTO);
            }
        }

        public class Get
        {
            [Fact]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSet = MockHelper.MockSet(new List<Data.Enemy>());

                var mockRepository = new Mock<NecroDancerContext>();
                mockRepository.Setup(x => x.Enemies).Returns(mockSet.Object);

                var controller = new EnemiesController(mockRepository.Object);

                // Act
                var actionResult = await controller.GetEnemies(new EnemiesPagination());
                var contentResult = actionResult as OkNegotiatedContentResult<Enemies>;

                // Assert
                Assert.NotNull(contentResult);
                Assert.NotNull(contentResult.Content);
            }
        }

        public class Get_Attribute
        {
            [Fact]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSet = MockHelper.MockSet(new List<Data.Enemy>());

                var mockRepository = new Mock<NecroDancerContext>();
                mockRepository.Setup(x => x.Enemies).Returns(mockSet.Object);

                var controller = new EnemiesController(mockRepository.Object);

                // Act
                var actionResult = await controller.GetEnemies(null, new EnemiesPagination());
                var contentResult = actionResult as OkNegotiatedContentResult<Enemies>;

                // Assert
                Assert.NotNull(contentResult);
                Assert.NotNull(contentResult.Content);
            }
        }
    }
}

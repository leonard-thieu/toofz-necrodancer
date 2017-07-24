using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ApiExceptionTests
    {
        public class CreateAsync
        {
            [Fact]
            public async Task ReturnsApiException()
            {
                // Arrange
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("{}"),
                    RequestMessage = new HttpRequestMessage(),
                };

                // Act
                ApiException apiEx = null;
                var ex = await Record.ExceptionAsync(async () =>
                {
                    apiEx = await ApiException.CreateAsync(response);
                });

                // Assert
                Assert.Null(ex);
                Assert.Null(apiEx.RequestUri);
                Assert.Null(apiEx.Request);
                Assert.Equal("{}", apiEx.Response);
                Assert.Equal(HttpStatusCode.BadRequest, apiEx.StatusCode);
            }

            [Fact]
            public async Task NullResponse_ThrowsArgumentNullException()
            {
                // Arrange
                HttpResponseMessage response = null;

                // Act
                var ex = await Record.ExceptionAsync(() => ApiException.CreateAsync(response));

                // Assert
                Assert.NotNull(ex);
                Assert.IsType<ArgumentNullException>(ex);
            }

            [Fact]
            public async Task SuccessResponse_ThrowsArgumentException()
            {
                // Arrange
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                // Act
                var ex = await Record.ExceptionAsync(() => ApiException.CreateAsync(response));

                // Assert
                Assert.NotNull(ex);
                Assert.IsType<ArgumentException>(ex);
            }
        }

        public class CreateIncorrectMediaTypeAsync
        {
            [Fact]
            public async Task ReturnsApiException()
            {
                // Arrange
                var response = new HttpResponseMessage();
                response.Content = new StringContent("", Encoding.UTF8, "application/xml");
                response.RequestMessage = new HttpRequestMessage { RequestUri = Constants.FakeUri };

                // Act
                var ex = await Record.ExceptionAsync(() => ApiException.CreateIncorrectMediaTypeAsync(response));

                // Assert
                Assert.Null(ex);
            }

            [Fact]
            public async Task NullResponse_ThrowsArgumentNullException()
            {
                // Arrange
                HttpResponseMessage response = null;

                // Act
                var ex = await Record.ExceptionAsync(() => ApiException.CreateIncorrectMediaTypeAsync(response));

                // Assert
                Assert.NotNull(ex);
                Assert.IsType<ArgumentNullException>(ex);
            }
        }
    }
}

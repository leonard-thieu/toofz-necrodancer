using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class EnsureSuccessHandlerTests
    {
        private static readonly CancellationToken Cancellation = CancellationToken.None;

        public class SendAsync
        {
            [Fact]
            public async Task Success_ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler.When("*").Respond(HttpStatusCode.OK);
                var handler = new TestingHttpMessageHandler(new EnsureSuccessHandler { InnerHandler = mockHandler });

                var mockRequest = new Mock<HttpRequestMessage>();

                // Act
                var ex = await Record.ExceptionAsync(async () =>
                {
                    await handler.TestSendAsync(mockRequest.Object, Cancellation);
                });

                // Assert
                Assert.Null(ex);
            }

            [Fact]
            public async Task NonSuccess_ThrowsApiException()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                var handler = new TestingHttpMessageHandler(new EnsureSuccessHandler { InnerHandler = mockHandler });

                var mockRequest = new Mock<HttpRequestMessage>();

                // Act
                var ex = await Record.ExceptionAsync(async () =>
                {
                    await handler.TestSendAsync(mockRequest.Object, Cancellation);
                });

                // Assert
                Assert.IsType<ApiException>(ex);
            }
        }
    }
}

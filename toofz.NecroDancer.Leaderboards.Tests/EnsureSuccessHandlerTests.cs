using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class EnsureSuccessHandlerTests
    {
        static readonly CancellationToken Cancellation = CancellationToken.None;

        [TestClass]
        public class SendAsync
        {
            [TestMethod]
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
                Assert.IsNull(ex);
            }

            [TestMethod]
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
                Assert.IsInstanceOfType(ex, typeof(ApiException));
            }
        }
    }
}

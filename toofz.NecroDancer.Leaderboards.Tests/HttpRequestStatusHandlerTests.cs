using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class HttpRequestStatusHandlerTests
    {
        [TestClass]
        public class SendAsync
        {
            [TestMethod]
            public async Task Success_ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.OK);

                var handler = new TestingHttpMessageHandler(new HttpRequestStatusHandler { InnerHandler = mockHandler });

                var mockRequest = new Mock<HttpRequestMessage>();

                // Act
                var response = await handler.TestSendAsync(mockRequest.Object);

                // Assert
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }

            [TestMethod]
            public async Task NonSuccess_ThrowsHttpRequestStatusException()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.BadRequest, "application/json", "");

                var handler = new TestingHttpMessageHandler(new HttpRequestStatusHandler { InnerHandler = mockHandler });

                var mockRequest = new Mock<HttpRequestMessage>();

                // Act
                var ex = await Record.ExceptionAsync(async () =>
                {
                    await handler.TestSendAsync(mockRequest.Object);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(HttpRequestStatusException));
                var e = (HttpRequestStatusException)ex;
                Assert.AreEqual(e.StatusCode, HttpStatusCode.BadRequest);
            }
        }
    }
}

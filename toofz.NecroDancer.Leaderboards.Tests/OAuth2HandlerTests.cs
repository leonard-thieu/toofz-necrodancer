using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class OAuth2HandlerTests
    {
        [TestClass]
        public class SendAsync
        {
            [TestMethod]
            public async Task Authorized_ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler.When("*").Respond(HttpStatusCode.OK);

                var bearerToken = new OAuth2AccessToken();

                var handler = new TestingHttpMessageHandler(new OAuth2Handler { InnerHandler = mockHandler, BearerToken = bearerToken });

                var mockRequest = new Mock<HttpRequestMessage>();

                // Act
                await handler.TestSendAsync(mockRequest.Object, CancellationToken.None);
                var authorization = mockRequest.Object.Headers.Authorization;

                // Assert
                Assert.AreEqual("Bearer", authorization.Scheme);
            }

            [TestMethod]
            public async Task Unauthorized_Authenticates()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();

                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Unauthorized
                };
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer"));
                mockHandler.Expect(Constants.FakeUri)
                    .Respond(response);

                mockHandler.Expect(new Uri(Constants.FakeUri, "/token"))
                    .RespondJson(new OAuth2AccessToken
                    {
                        AccessToken = "fakeToken",
                        TokenType = "bearer",
                    });

                mockHandler.Expect(Constants.FakeUri)
                    .WithHeaders("Authorization", "Bearer fakeToken")
                    .Respond(HttpStatusCode.OK);

                var handler = new TestingHttpMessageHandler(new OAuth2Handler { InnerHandler = mockHandler });

                var request = new HttpRequestMessage()
                {
                    RequestUri = Constants.FakeUri
                };

                // Act
                await handler.TestSendAsync(request, CancellationToken.None);
                var authorization = request.Headers.Authorization;

                // Assert
                mockHandler.VerifyNoOutstandingExpectation();
                Assert.AreEqual("Bearer", authorization.Scheme);
            }
        }
    }
}

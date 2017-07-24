using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class OAuth2HandlerTests
    {
        private static readonly Uri BaseAddress = new Uri("http://fake.domain.tld/", UriKind.Absolute);
        private static readonly CancellationToken Cancellation = CancellationToken.None;

        public class SendAsync
        {
            [Fact]
            public async Task Authorized_ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler.When("*").Respond(HttpStatusCode.OK);

                var bearerToken = new OAuth2AccessToken();

                var handler = new TestingHttpMessageHandler(new OAuth2Handler { InnerHandler = mockHandler, BearerToken = bearerToken });

                var mockRequest = new Mock<HttpRequestMessage>();

                // Act
                await handler.TestSendAsync(mockRequest.Object, Cancellation);
                var authorization = mockRequest.Object.Headers.Authorization;

                // Assert
                Assert.Equal("Bearer", authorization.Scheme);
            }

            [Fact]
            public async Task Unauthorized_Authenticates()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();

                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer"));
                mockHandler.Expect(BaseAddress)
                    .Respond(response);

                mockHandler.Expect(new Uri(BaseAddress, "/token"))
                    .RespondJson(new OAuth2AccessToken
                    {
                        AccessToken = "fakeToken",
                        TokenType = "bearer",
                    });

                mockHandler.Expect(BaseAddress)
                    .WithHeaders("Authorization", "Bearer fakeToken")
                    .Respond(HttpStatusCode.OK);

                var handler = new TestingHttpMessageHandler(new OAuth2Handler { InnerHandler = mockHandler });

                var request = new HttpRequestMessage();
                request.RequestUri = BaseAddress;

                // Act
                await handler.TestSendAsync(request, Cancellation);
                var authorization = request.Headers.Authorization;

                // Assert
                mockHandler.VerifyNoOutstandingExpectation();
                Assert.Equal("Bearer", authorization.Scheme);
            }
        }
    }
}

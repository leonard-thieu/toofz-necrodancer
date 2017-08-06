using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class ApiExceptionTests
    {
        [TestClass]
        public class CreateAsync
        {
            [TestMethod]
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
                Assert.IsNull(ex);
                Assert.IsNull(apiEx.RequestUri);
                Assert.IsNull(apiEx.RequestContent);
                Assert.AreEqual("{}", apiEx.ResponseContent);
                Assert.AreEqual(HttpStatusCode.BadRequest, apiEx.StatusCode);
            }

            [TestMethod]
            public async Task NullResponse_ThrowsArgumentNullException()
            {
                // Arrange
                HttpResponseMessage response = null;

                // Act
                var ex = await Record.ExceptionAsync(() => ApiException.CreateAsync(response));

                // Assert
                Assert.IsNotNull(ex);
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task SuccessResponse_ThrowsArgumentException()
            {
                // Arrange
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                // Act
                var ex = await Record.ExceptionAsync(() => ApiException.CreateAsync(response));

                // Assert
                Assert.IsNotNull(ex);
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestClass]
        public class CreateIncorrectMediaTypeAsync
        {
            [TestMethod]
            public async Task ReturnsApiException()
            {
                // Arrange
                var response = new HttpResponseMessage()
                {
                    Content = new StringContent("", Encoding.UTF8, "application/xml"),
                    RequestMessage = new HttpRequestMessage { RequestUri = Constants.FakeUri }
                };

                // Act
                var ex = await Record.ExceptionAsync(() => ApiException.CreateIncorrectMediaTypeAsync(response));

                // Assert
                Assert.IsNull(ex);
            }

            [TestMethod]
            public async Task NullResponse_ThrowsArgumentNullException()
            {
                // Arrange
                HttpResponseMessage response = null;

                // Act
                var ex = await Record.ExceptionAsync(() => ApiException.CreateIncorrectMediaTypeAsync(response));

                // Assert
                Assert.IsNotNull(ex);
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }
    }
}

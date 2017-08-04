using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class UgcHttpClientTests
    {
        [TestClass]
        public class GetUgcFileAsyncTests
        {
            [TestMethod]
            public async Task UrlIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();

                var ugcHttpClient = new UgcHttpClient(handler);

                // Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return ugcHttpClient.GetUgcFileAsync(null);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public async Task ValidParams_ReturnsUgcFile()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(HttpMethod.Get, "http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/")
                    .Respond("application/octet-stream", Resources.UgcFile);

                var ugcHttpClient = new UgcHttpClient(handler);

                // Act
                var ugcFile = await ugcHttpClient.GetUgcFileAsync("http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/");

                // Assert
                Assert.IsInstanceOfType(ugcFile, typeof(Stream));
            }
        }
    }
}

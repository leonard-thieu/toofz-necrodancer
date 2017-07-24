using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class StreamPipelineTests
    {
        public class CreateRequestBlock
        {
            [Fact]
            public async Task Returns_Response()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.Expect(Constants.FakeUri).Respond(new StringContent("fakeContent"));

                var httpClient = new HttpClient(handler);

                var request = StreamPipeline.CreateRequestBlock(httpClient, CancellationToken.None);

                // Act
                var sent = await request.SendAsync(Constants.FakeUri);
                request.Complete();
                HttpResponseMessage response = await request.ReceiveAsync();
                await request.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);
                handler.VerifyNoOutstandingExpectation();
            }
        }

        public class CreateDownloadBlock
        {
            [Fact]
            public async Task Returns_Content()
            {
                // Arrange
                var response = new HttpResponseMessage { Content = new StringContent("fakeContent") };

                var download = StreamPipeline.CreateDownloadBlock();

                // Act
                var sent = await download.SendAsync(response);
                download.Complete();
                var httpContent = await download.ReceiveAsync();
                var content = await httpContent.ReadAsStringAsync();
                await download.Completion;

                // Assert
                Assert.True(sent);
                Assert.Equal("fakeContent", content);
            }
        }

        public class CreateProcessContentBlock
        {
            [Fact]
            public async Task Returns_Stream()
            {
                // Arrange
                var httpContent = new StringContent("fakeContent");

                var processContent = StreamPipeline.CreateProcessContentBlock(null);

                // Act
                var sent = await processContent.SendAsync(httpContent);
                processContent.Complete();
                var stream = await processContent.ReceiveAsync();
                await processContent.Completion;

                // Assert
                Assert.True(sent);
                Assert.True(stream.CanRead);
                Assert.True(stream.CanSeek);
            }
        }
    }
}

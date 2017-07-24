using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ReplayPipelineTests
    {
        public class CreateDownloadDetails
        {
            [Fact]
            public async Task Returns_ReplayDetails()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.Expect(Constants.FakeUri).Respond(new StringContent("fakeContent"));

                var httpClient = new HttpClient(handler);

                var downloadDetails = ReplayPipeline.CreateDownloadDetails(httpClient, CancellationToken.None);

                ReplayContext response = null;
                var getResult = new ActionBlock<ReplayContext>(r => response = r);

                downloadDetails.LinkTo(getResult, new DataflowLinkOptions { PropagateCompletion = true });

                // Act
                var sent = await downloadDetails.SendAsync(new Tuple<long, Uri>(0, Constants.FakeUri));
                downloadDetails.Complete();
                await getResult.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);
                Assert.NotNull(response.DetailsContent);
                handler.VerifyNoOutstandingExpectation();
            }

            [Fact]
            public async Task NotFound_SetsErrorCode()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.Expect(Constants.FakeUri).Respond(HttpStatusCode.NotFound);

                var httpClient = new HttpClient(handler);

                var downloadDetails = ReplayPipeline.CreateDownloadDetails(httpClient, CancellationToken.None);

                ReplayContext response = null;
                var getResult = new ActionBlock<ReplayContext>(r => response = r);

                downloadDetails.LinkTo(getResult, new DataflowLinkOptions { PropagateCompletion = true });

                // Act
                var sent = await downloadDetails.SendAsync(new Tuple<long, Uri>(0, Constants.FakeUri));
                downloadDetails.Complete();

                await getResult.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);
                Assert.Equal(404, response.ErrorCode);
                handler.VerifyNoOutstandingExpectation();
            }

            [Fact]
            public async Task Non404NonSuccess_ThrowsHttpRequestException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.Expect(Constants.FakeUri).Respond(HttpStatusCode.Forbidden);

                var httpClient = new HttpClient(handler);

                var downloadDetails = ReplayPipeline.CreateDownloadDetails(httpClient, CancellationToken.None);

                // Act
                var sent = await downloadDetails.SendAsync(new Tuple<long, Uri>(0, Constants.FakeUri));
                downloadDetails.Complete();
                var ex = await Record.ExceptionAsync(() => downloadDetails.Completion);

                // Assert
                Assert.True(sent);
                Assert.NotNull(ex);
                Assert.IsType<HttpRequestException>(ex);
                handler.VerifyNoOutstandingExpectation();
            }
        }

        public class ProcessDetails
        {
            [Fact]
            public async Task Returns_DataUri()
            {
                // Arrange
                var context = new ReplayContext { DetailsContent = new StringContent($"<data><url>{Constants.FakeUri}</url></data>") };

                var processDetails = ReplayPipeline.ProcessDetails(null);

                ReplayContext response = null;
                var getResult = new ActionBlock<ReplayContext>(r => response = r);

                processDetails.LinkTo(getResult, new DataflowLinkOptions { PropagateCompletion = true });

                // Act
                var sent = await processDetails.SendAsync(context);
                processDetails.Complete();
                await getResult.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);

                var dataUri = response.DataUri;
                Assert.Equal(Constants.FakeUri, dataUri);
            }

            [Fact]
            public async Task NullDetails_DoesNothing()
            {
                // Arrange
                var context = new ReplayContext();

                var processDetails = ReplayPipeline.ProcessDetails(null);

                ReplayContext response = null;
                var getResult = new ActionBlock<ReplayContext>(r => response = r);

                processDetails.LinkTo(getResult, new DataflowLinkOptions { PropagateCompletion = true });

                // Act
                var sent = await processDetails.SendAsync(context);
                processDetails.Complete();
                await getResult.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);

                Assert.Null(response.DataUri);
            }
        }

        public class CreateDownloadData
        {
            [Fact]
            public async Task Returns_DataContent()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.Expect(Constants.FakeUri).Respond(new StringContent("fakeContent"));

                var httpClient = new HttpClient(handler);

                var downloadData = ReplayPipeline.CreateDownloadData(httpClient, CancellationToken.None);

                ReplayContext response = null;
                var getResult = new ActionBlock<ReplayContext>(r => response = r);

                downloadData.LinkTo(getResult, new DataflowLinkOptions { PropagateCompletion = true });

                var context = new ReplayContext { DataUri = Constants.FakeUri };

                // Act
                var sent = await downloadData.SendAsync(context);
                downloadData.Complete();
                await getResult.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);
                Assert.NotNull(response.DataContent);
                handler.VerifyNoOutstandingExpectation();
            }

            [Fact]
            public async Task NullDataUri_DoesNothing()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();

                var httpClient = new HttpClient(handler);

                var downloadData = ReplayPipeline.CreateDownloadData(httpClient, CancellationToken.None);

                ReplayContext response = null;
                var getResult = new ActionBlock<ReplayContext>(r => response = r);

                downloadData.LinkTo(getResult, new DataflowLinkOptions { PropagateCompletion = true });

                var context = new ReplayContext();

                // Act
                var sent = await downloadData.SendAsync(context);
                downloadData.Complete();
                await getResult.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);
                Assert.Null(response.DataContent);
            }

            [Fact]
            public async Task NotFound_SetsErrorCode()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.Expect(Constants.FakeUri).Respond(HttpStatusCode.NotFound);

                var httpClient = new HttpClient(handler);

                var downloadData = ReplayPipeline.CreateDownloadData(httpClient, CancellationToken.None);

                ReplayContext response = null;
                var getResult = new ActionBlock<ReplayContext>(r => response = r);

                downloadData.LinkTo(getResult, new DataflowLinkOptions { PropagateCompletion = true });

                var context = new ReplayContext { DataUri = Constants.FakeUri };

                // Act
                var sent = await downloadData.SendAsync(context);
                downloadData.Complete();
                await getResult.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);
                Assert.Equal(-404, response.ErrorCode);
                handler.VerifyNoOutstandingExpectation();
            }

            [Fact]
            public async Task Non404NonSuccess_ThrowsHttpRequestException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler.Expect(Constants.FakeUri).Respond(HttpStatusCode.Forbidden);

                var httpClient = new HttpClient(handler);

                var downloadData = ReplayPipeline.CreateDownloadData(httpClient, CancellationToken.None);

                var context = new ReplayContext { DataUri = Constants.FakeUri };

                // Act
                var sent = await downloadData.SendAsync(context);
                downloadData.Complete();
                var ex = await Record.ExceptionAsync(() => downloadData.Completion);

                // Assert
                Assert.True(sent);
                Assert.NotNull(ex);
                Assert.IsType<HttpRequestException>(ex);
                handler.VerifyNoOutstandingExpectation();
            }
        }

        public class CreateProcessData
        {
            [Fact]
            public async Task Returns_Data()
            {
                // Arrange
                var processData = ReplayPipeline.CreateProcessData(null);

                ReplayContext response = null;
                var getResult = new ActionBlock<ReplayContext>(r => response = r);

                processData.LinkTo(getResult, new DataflowLinkOptions { PropagateCompletion = true });

                var context = new ReplayContext { DataContent = new StringContent("fakeContent") };

                // Act
                var sent = await processData.SendAsync(context);
                processData.Complete();
                await getResult.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);
                Assert.NotNull(response.Data);
            }

            [Fact]
            public async Task NullDataContent_DoesNothing()
            {
                // Arrange
                var processData = ReplayPipeline.CreateProcessData(null);

                ReplayContext response = null;
                var getResult = new ActionBlock<ReplayContext>(r => response = r);

                processData.LinkTo(getResult, new DataflowLinkOptions { PropagateCompletion = true });

                var context = new ReplayContext();

                // Act
                var sent = await processData.SendAsync(context);
                processData.Complete();
                await getResult.Completion;

                // Assert
                Assert.True(sent);
                Assert.NotNull(response);
                Assert.Null(response.Data);
            }
        }
    }
}

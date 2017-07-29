using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ReplayPipelineTests
    {
        [TestClass]
        public class CreateDownloadDetails
        {
            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.DetailsContent);
                handler.VerifyNoOutstandingExpectation();
            }

            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(response);
                Assert.AreEqual(404, response.ErrorCode);
                handler.VerifyNoOutstandingExpectation();
            }

            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(ex);
                Assert.IsInstanceOfType(ex, typeof(HttpRequestException));
                handler.VerifyNoOutstandingExpectation();
            }
        }

        [TestClass]
        public class ProcessDetails
        {
            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(response);

                var dataUri = response.DataUri;
                Assert.AreEqual(Constants.FakeUri, dataUri);
            }

            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(response);

                Assert.IsNull(response.DataUri);
            }
        }

        [TestClass]
        public class CreateDownloadData
        {
            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.DataContent);
                handler.VerifyNoOutstandingExpectation();
            }

            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(response);
                Assert.IsNull(response.DataContent);
            }

            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(response);
                Assert.AreEqual(-404, response.ErrorCode);
                handler.VerifyNoOutstandingExpectation();
            }

            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(ex);
                Assert.IsInstanceOfType(ex, typeof(HttpRequestException));
                handler.VerifyNoOutstandingExpectation();
            }
        }

        [TestClass]
        public class CreateProcessData
        {
            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Data);
            }

            [TestMethod]
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
                Assert.IsTrue(sent);
                Assert.IsNotNull(response);
                Assert.IsNull(response.Data);
            }
        }
    }
}

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    sealed class TestingHttpMessageHandler : DelegatingHandler
    {
        public TestingHttpMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        public Task<HttpResponseMessage> TestSendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsync(request, cancellationToken);
        }
    }
}

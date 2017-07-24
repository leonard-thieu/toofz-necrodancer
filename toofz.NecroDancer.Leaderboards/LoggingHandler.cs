using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LoggingHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoggingHandler));

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Log.Debug($"Start download {request.RequestUri}");
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            Log.Debug($"End download {request.RequestUri}");

            return response;
        }
    }
}

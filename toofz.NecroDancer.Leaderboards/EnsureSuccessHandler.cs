using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class EnsureSuccessHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EnsureSuccessHandler));

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="Task{HttpResponseMessage}"/>. The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="ApiException">
        /// The response returned a non-success status code.
        /// </exception>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var uri = request.RequestUri;
                var statusCode = (int)response.StatusCode;

                Log.Error($"{statusCode} {uri}");

                throw await ApiException.CreateAsync(response).ConfigureAwait(false);
            }

            return response;
        }
    }
}

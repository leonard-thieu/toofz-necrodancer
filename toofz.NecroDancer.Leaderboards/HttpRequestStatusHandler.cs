using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class HttpRequestStatusHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="Task{TResult}"/>. The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestStatusException">
        /// The response returned a non-success status code.
        /// </exception>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestStatusException(ex.Message, ex) { StatusCode = response.StatusCode };
            }

            return response;
        }
    }
}

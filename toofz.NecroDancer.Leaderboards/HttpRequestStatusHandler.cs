using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class HttpRequestStatusHandler : DelegatingHandler
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(HttpRequestStatusHandler));

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

            if (!response.IsSuccessStatusCode)
            {
                var message = $"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}).";
                var requestUri = request.RequestUri;
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                HttpError httpError = null;
                var serializer = JsonSerializer.CreateDefault();
                using (var sr = new StringReader(content))
                {
                    httpError = (HttpError)serializer.Deserialize(sr, typeof(HttpError));
                }

                var ex = new HttpRequestStatusException(message)
                {
                    StatusCode = response.StatusCode,
                    RequestUri = requestUri,
                };
                if (httpError == null)
                {
                    ex.ResponseContent = content;
                }
                else
                {
                    Log.Error(message, httpError.ToHttpErrorException());
                }

                throw ex;
            }

            return response;
        }
    }
}

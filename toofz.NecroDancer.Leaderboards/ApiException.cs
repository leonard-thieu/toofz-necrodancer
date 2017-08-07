using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents errors returned from the API server.
    /// </summary>
    public sealed class ApiException : Exception
    {
        #region Static Members

        /// <summary>
        /// Creates an instance of <see cref="ApiException"/> from an unsuccessful HTTP response.
        /// </summary>
        /// <param name="response">The unsuccessful HTTP response.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronus operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="response"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="response"/> is a successful response.
        /// </exception>
        public static Task<ApiException> CreateAsync(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response), $"{nameof(response)} is null.");
            if (response.IsSuccessStatusCode)
                throw new ArgumentException($"Cannot create {nameof(ApiException)} from a successful response.", nameof(response));

            var message = "Received an error response from the server.";

            return CreateAsync(response, message);
        }

        /// <summary>
        /// Creates an instance of <see cref="ApiException"/> from a response with an unexpected media type.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronus operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="response"/> is null.
        /// </exception>
        public static Task<ApiException> CreateIncorrectMediaTypeAsync(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response), $"{nameof(response)} is null.");

            var mediaType = response.Content.Headers.ContentType.MediaType;
            var message = $"Expected to receive a response with content type 'application/json' but received '{mediaType}' instead.";

            return CreateAsync(response, message);
        }

        static async Task<ApiException> CreateAsync(HttpResponseMessage response, string message)
        {
            var requestUri = response.RequestMessage.RequestUri;

            string requestContent = null;
            try
            {
                var content = response.RequestMessage.Content;
                if (content != null)
                {
                    requestContent = await content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (ObjectDisposedException)
            {
                requestContent = "Could not read request.";
            }

            string responseContent = null;
            try
            {
                var content = response.Content;
                if (content != null)
                {
                    responseContent = await content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (ObjectDisposedException)
            {
                responseContent = "Could not read response.";
            }

            // This must be called after retrieving the response because it disposes it.
            var ex = GetHttpRequestException(response);

            return new ApiException(message, ex, requestUri, requestContent, responseContent, response.StatusCode);
        }

        static HttpRequestException GetHttpRequestException(HttpResponseMessage response)
        {
            HttpRequestException httpEx = null;

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                httpEx = ex;
            }

            return httpEx;
        }

        #endregion

        #region Instance Members

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class with a specified error message and 
        /// a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) 
        /// if no inner exception is specified.
        /// </param>
        /// <param name="requestUri">
        /// The URI of the request.
        /// </param>
        /// <param name="requestContent">
        /// The contents of the request.
        /// </param>
        /// <param name="responseContent">
        /// The contents of the response.
        /// </param>
        /// <param name="statusCode">
        /// The HTTP status code of the response.
        /// </param>
        ApiException(string message, Exception innerException, Uri requestUri, string requestContent, string responseContent, HttpStatusCode statusCode) : base(message, innerException)
        {
            RequestUri = requestUri;
            RequestContent = requestContent;
            ResponseContent = responseContent;
            StatusCode = statusCode;
        }

        /// <summary>
        /// The URI of the request.
        /// </summary>
        public Uri RequestUri { get; }
        /// <summary>
        /// The contents of the request.
        /// </summary>
        public string RequestContent { get; }
        /// <summary>
        /// The contents of the response.
        /// </summary>
        public string ResponseContent { get; }
        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        #endregion
    }
}

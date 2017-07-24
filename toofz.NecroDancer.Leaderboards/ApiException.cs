﻿using System;
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
        /// A <see cref="Task{ApiException}"/> that represents the asynchronus operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="response"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="response"/> is a successful response.
        /// </exception>
        public static Task<ApiException> CreateAsync(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));
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
        /// A <see cref="Task{ApiException}"/> that represents the asynchronus operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="response"/> is null.
        /// </exception>
        public static Task<ApiException> CreateIncorrectMediaTypeAsync(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var mediaType = response.Content.Headers.ContentType.MediaType;
            var message = $"Expected to receive a response with content type 'application/json' but received '{mediaType}' instead.";

            return CreateAsync(response, message);
        }

        private static async Task<ApiException> CreateAsync(HttpResponseMessage response, string message)
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

        private static HttpRequestException GetHttpRequestException(HttpResponseMessage response)
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
        /// <param name="response">The error response.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) 
        /// if no inner exception is specified.
        /// </param>
        private ApiException(string message, Exception innerException, Uri requestUri, string request, string response, HttpStatusCode statusCode) : base(message, innerException)
        {
            RequestUri = requestUri;
            Request = request;
            Response = response;
            StatusCode = statusCode;
        }

        /// <summary>
        /// The request URI.
        /// </summary>
        public Uri RequestUri { get; }
        /// <summary>
        /// The request.
        /// </summary>
        public string Request { get; }
        /// <summary>
        /// The error response.
        /// </summary>
        public string Response { get; }
        /// <summary>
        /// The HTTP status code of the response that caused the exception.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        #endregion
    }
}

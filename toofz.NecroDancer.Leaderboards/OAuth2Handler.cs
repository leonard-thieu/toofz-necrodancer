using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class OAuth2Handler : DelegatingHandler
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(OAuth2Handler));
        static readonly AuthenticationHeaderValue BearerHeader = new AuthenticationHeaderValue("Bearer");

        public string UserName { get; set; }
        public string Password { get; set; }

        public OAuth2AccessToken BearerToken { get; internal set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddBearerToken(request);
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (!response.Headers.WwwAuthenticate.Contains(BearerHeader))
                {
                    Log.Warn("Did not receive 'WWW-Authenticate: Bearer' header.");
                }
                else
                {
                    BearerToken = await AuthenticateAsync(request.RequestUri, cancellationToken).ConfigureAwait(false);
                    AddBearerToken(request);
                    response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                }
            }

            return response;
        }

        void AddBearerToken(HttpRequestMessage request)
        {
            if (BearerToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken.AccessToken);
            }
        }

        async Task<OAuth2AccessToken> AuthenticateAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            var authUri = new Uri(requestUri, "/token");
            Log.Info($"Authenticating to '{authUri}'...");

            var loginData = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "userName", UserName },
                { "password", Password },
            };
            var content = new FormUrlEncodedContent(loginData);

            HttpResponseMessage response = await PostAsync(authUri, content, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiException.CreateAsync(response).ConfigureAwait(false);
            }

            var accessToken = await response.Content.ReadAsAsync<OAuth2AccessToken>(cancellationToken).ConfigureAwait(false);

            if (!(accessToken.TokenType == "bearer" && accessToken.UserName == UserName))
            {
                throw new InvalidDataException("Did not receive a valid bearer token.");
            }

            return accessToken;
        }

        Task<HttpResponseMessage> PostAsync(Uri requestUri, FormUrlEncodedContent content, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = content;

            return SendAsync(request, cancellationToken);
        }
    }
}

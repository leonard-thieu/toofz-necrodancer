using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using toofz.NecroDancer.Web.Api.Identity;
using toofz.NecroDancer.Web.Api.Providers;

namespace toofz.NecroDancer.Web.Api
{
    partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                AllowInsecureHttp = false,
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}

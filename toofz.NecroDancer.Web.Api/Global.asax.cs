using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;

namespace toofz.NecroDancer.Web.Api
{
    /// <summary>
    /// The web API application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebApiApplication : HttpApplication
    {
        public static string InstrumentationKey
        {
            get { return TelemetryConfiguration.Active.InstrumentationKey; }
            private set
            {
                if (value == null)
                {
                    TelemetryConfiguration.Active.InstrumentationKey = "";
                    TelemetryConfiguration.Active.DisableTelemetry = true;
                }
                else
                {
                    TelemetryConfiguration.Active.InstrumentationKey = value;
                    TelemetryConfiguration.Active.DisableTelemetry = false;
                }
            }
        }


        /// <summary>
        /// Performs application configuration.
        /// </summary>
        protected void Application_Start()
        {
            InstrumentationKey = Environment.GetEnvironmentVariable("toofzApiInstrumentationKey", EnvironmentVariableTarget.Machine);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MvcHandler.DisableMvcResponseHeader = true;
        }
    }
}

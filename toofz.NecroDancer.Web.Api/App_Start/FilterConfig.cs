using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace toofz.NecroDancer.Web.Api
{
    [ExcludeFromCodeCoverage]
    internal static class FilterConfig
    {
        private static TelemetryClient TelemetryClient = new TelemetryClient();

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute(TelemetryClient));
        }
    }
}
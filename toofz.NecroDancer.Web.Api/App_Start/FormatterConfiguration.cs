using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;

namespace toofz.NecroDancer.Web.Api
{
    [ExcludeFromCodeCoverage]
    internal static class FormatterConfiguration
    {
        public static void Configure(HttpConfiguration config)
        {
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            var settings = formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            config.Formatters.Add(new BrowserJsonFormatter(settings));
        }
    }
}
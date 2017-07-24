using System.Web.Mvc;
using System.Web.Routing;

namespace toofz.NecroDancer.WebClient
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{*tags}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}

using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using RazorGenerator.Mvc;
using toofz.NecroDancer.Web.Api;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(RazorGeneratorMvcStart), "Start")]

namespace toofz.NecroDancer.Web.Api
{
    public static class RazorGeneratorMvcStart
    {
        public static void Start()
        {
            var engine = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).GetTypeInfo().Assembly)
            {
                UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
            };

            ViewEngines.Engines.Insert(0, engine);

            // StartPage lookups are done by WebPages. 
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }
    }
}

using System.Diagnostics.CodeAnalysis;
using System.Web.Optimization;

namespace toofz.NecroDancer.Web.Api
{
    [ExcludeFromCodeCoverage]
    internal static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
#if !DEBUG
            BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;

            Scripts.DefaultTagFormat = "<script src=\"{0}\" crossorigin=\"anonymous\"></script>";
#endif
            bundles.Add(new StyleBundle("~/css/bootstrap", "https://maxcdn.bootstrapcdn.com/bootswatch/3.3.6/slate/bootstrap.min.css").Include(
                "~/Content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/js/jquery", "https://code.jquery.com/jquery-3.1.1.min.js").Include(
                "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/js/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js").Include(
                "~/Scripts/bootstrap.js"));
        }
    }
}

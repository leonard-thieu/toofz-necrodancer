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
        }
    }
}

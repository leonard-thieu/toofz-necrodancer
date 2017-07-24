using System.Diagnostics.CodeAnalysis;
using Owin;

namespace toofz.NecroDancer.Web.Api
{
    [ExcludeFromCodeCoverage]
    internal sealed partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

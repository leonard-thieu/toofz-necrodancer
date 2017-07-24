using System.Web;

namespace toofz.NecroDancer.Web.Api
{
    public class HttpServerUtilityWrapper : IHttpServerUtilityWrapper
    {
        public string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}

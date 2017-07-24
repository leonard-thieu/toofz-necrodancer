using System.Web.Mvc;

namespace toofz.NecroDancer.WebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new FilePathResult($"~/index.html", "text/html");
        }
    }
}
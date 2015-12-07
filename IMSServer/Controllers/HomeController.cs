using System.Web.Mvc;

namespace IMSServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public static int Sum(int a, int b) => a + b;
    }
}

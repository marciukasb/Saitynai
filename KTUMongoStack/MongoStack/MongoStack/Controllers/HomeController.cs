using System.Web.Mvc;

namespace MongoStack.Controllers
{
   
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
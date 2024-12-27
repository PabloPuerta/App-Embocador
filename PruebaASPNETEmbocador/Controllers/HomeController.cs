using System.Linq;
using System.Web.Mvc;
using PruebaASPNETEmbocador.Models;

namespace PruebaASPNETEmbocador.Controllers
{
    public class HomeController : Controller
    {
     
        public ActionResult Index()
        {
      
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}


using System.Linq;
using System.Web.Mvc;
using Tradutor.DAL;
using Tradutor.Models;

namespace Tradutor.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home/Index
        public ActionResult Index()
        {
            // Usa db herdado do BaseController
            var idiomas = db.Idiomas.ToList();
            ViewBag.IdiomasAdicionais = idiomas;

            return View();
        }

        // GET: Home/About
        public ActionResult About()
        {
            var idiomas = db.Idiomas.ToList();
            ViewBag.IdiomasAdicionais = idiomas;

            ViewBag.Message = "Your application description page.";
            return View();
        }

        // GET: Home/Contact
        public ActionResult Contact()
        {
            var idiomas = db.Idiomas.ToList();
            ViewBag.IdiomasAdicionais = idiomas;

            ViewBag.Message = "Your contact page.";
            return View();
        }

        // 
    }
}

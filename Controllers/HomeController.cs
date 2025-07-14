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
            // Define idioma padrão na sessão, se não estiver definido
            if (Session["Idioma"] == null)
            {
                Session["Idioma"] = "pt"; // ou outro idioma padrão que você queira
            }

            // Usa db herdado do BaseController para pegar todos idiomas
            var idiomas = db.Idiomas.ToList();
            ViewBag.IdiomasAdicionais = idiomas;

            return View();
        }

        // GET: Home/About
        public ActionResult About()
        {
            if (Session["Idioma"] == null)
            {
                Session["Idioma"] = "pt";
            }

            var idiomas = db.Idiomas.ToList();
            ViewBag.IdiomasAdicionais = idiomas;

            ViewBag.Message = "Your application description page.";
            return View();
        }

        // GET: Home/Contact
        public ActionResult Contact()
        {
            if (Session["Idioma"] == null)
            {
                Session["Idioma"] = "pt";
            }

            var idiomas = db.Idiomas.ToList();
            ViewBag.IdiomasAdicionais = idiomas;

            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}

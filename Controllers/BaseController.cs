using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tradutor.DAL;
using Tradutor.Models;

namespace Tradutor.Controllers
{
    public class BaseController : Controller
    {
        protected AppDbContext db = new AppDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Carrega apenas os idiomas do banco de dados
            var idiomasCompletos = db.Idiomas.ToList();

            ViewBag.IdiomasCompletos = idiomasCompletos;

            base.OnActionExecuting(filterContext);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}

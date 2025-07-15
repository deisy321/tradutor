using System.Linq;
using System.Web.Mvc;
using Tradutor.DAL;
using Tradutor.Helpers;
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
                Session["Idioma"] = "pt"; // ou outro idioma padrão
            }

            // Pega idioma atual
            string idioma = Session["Idioma"].ToString().ToLower();

            // Testes de tradução
            string fraseOriginal = "Seja bem-vindo ao sistema";
            string palavraOriginal = "Obrigado";

            string fraseTraduzida = TradutorHelper.Traduzir(fraseOriginal, idioma);
            string palavraTraduzida = TradutorHelper.Traduzir(palavraOriginal, idioma);

            // Envia resultados para a view
            ViewBag.IdiomaAtual = idioma;
            ViewBag.TesteFrase = fraseTraduzida;
            ViewBag.TestePalavra = palavraTraduzida;

            // Lista de idiomas para exibir seletor
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

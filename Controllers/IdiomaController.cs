using System;
using System.Web;
using System.Web.Mvc;

namespace Tradutor.Controllers
{
    public class IdiomaController : Controller
    {
        public ActionResult Trocar(string lang, string returnUrl)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                // Salva o idioma na sessão
                Session["Idioma"] = lang;

                // Salva o idioma em um cookie para persistência
                HttpCookie cookie = new HttpCookie("_lang", lang);
                cookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cookie);
            }

            // Redireciona de volta para onde o usuário estava
            return Redirect(returnUrl ?? "/");
        }
    }
}

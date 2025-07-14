using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tradutor.DAL;
using Tradutor.Models;

namespace Tradutor.Controllers
{
    public class IdiomaController : BaseController
    {
        // GET: Idioma
        public ActionResult Index()
        {
            // Busca apenas os idiomas que estão no banco de dados
            var idiomasDoBanco = db.Idiomas.ToList();
            return View(idiomasDoBanco);
        }

        // GET: Idioma/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Idioma/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Idioma idioma)
        {
            if (ModelState.IsValid)
            {
                db.Idiomas.Add(idioma);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(idioma);
        }

        // GET: Idioma/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Idioma idioma = db.Idiomas.Find(id);
            if (idioma == null)
                return HttpNotFound();

            return View(idioma);
        }

        // POST: Idioma/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Idioma idioma)
        {
            if (ModelState.IsValid)
            {
                db.Entry(idioma).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(idioma);
        }

        // GET: Idioma/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Idioma idioma = db.Idiomas.Find(id);
            if (idioma == null)
                return HttpNotFound();

            return View(idioma);
        }

        // POST: Idioma/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Idioma idioma = db.Idiomas.Find(id);
            db.Idiomas.Remove(idioma);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Idioma/Trocar?lang=xx&returnUrl=...
        public ActionResult Trocar(string lang, string returnUrl)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                // Atualiza a sessão com o idioma escolhido
                Session["Idioma"] = lang;

                // Cria ou atualiza o cookie _lang com validade de 1 ano
                HttpCookie cookie = new HttpCookie("_lang", lang)
                {
                    Expires = System.DateTime.Now.AddYears(1),
                    HttpOnly = true
                };
                Response.Cookies.Add(cookie);
            }

            // Se returnUrl não for seguro, redireciona para home
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home");
            }

            return Redirect(returnUrl);
        }
    }
}

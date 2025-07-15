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
                bool existe = db.Idiomas.Any(i => i.Codigo.ToLower() == idioma.Codigo.ToLower());
                if (existe)
                {
                    ModelState.AddModelError("Codigo", "Idioma já cadastrado com este código.");
                    return View(idioma);
                }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Idioma idioma)
        {
            // Validação: verifica se já existe outro idioma com o mesmo código (ignorando o atual)
            bool existe = db.Idiomas.Any(i => i.Codigo.ToLower() == idioma.Codigo.ToLower() && i.Id != idioma.Id);
            if (existe)
            {
                ModelState.AddModelError("Codigo", "Já existe outro idioma com este código.");
                return View(idioma);
            }

            if (ModelState.IsValid)
            {
                // Busca o objeto original no banco pelo Id
                var idiomaOriginal = db.Idiomas.Find(idioma.Id);
                if (idiomaOriginal == null)
                {
                    return HttpNotFound();
                }

                // Atualiza as propriedades do objeto original
                idiomaOriginal.Codigo = idioma.Codigo;
                idiomaOriginal.Descricao = idioma.Descricao;
                idiomaOriginal.BandeiraUrl = idioma.BandeiraUrl;

                // Salva alterações no banco
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
                Session["Idioma"] = lang;

                HttpCookie cookie = new HttpCookie("_lang", lang)
                {
                    Expires = System.DateTime.Now.AddYears(1),
                    HttpOnly = true
                };
                Response.Cookies.Add(cookie);
            }

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home");
            }

            return Redirect(returnUrl);
        }
    }
}

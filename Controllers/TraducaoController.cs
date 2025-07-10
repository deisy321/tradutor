using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tradutor.Models;

namespace Tradutor.Controllers
{
    public class TraducaoController : BaseController
    {
        // GET: Traducao
        public ActionResult Index()
        {
            var traducoes = db.Traducoes.Include(t => t.Idioma).OrderBy(t => t.Idioma.Descricao).ThenBy(t => t.TextoOriginal).ToList();
            return View(traducoes);
        }

        // GET: Traducao/Create?idiomaId=1
        public ActionResult Create(int? idiomaId)
        {
            if (idiomaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", idiomaId);
            var traducao = new Traducao { IdiomaId = idiomaId.Value };
            return View(traducao);
        }

        // POST: Traducao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Traducao traducao)
        {
            if (ModelState.IsValid)
            {
                db.Traducoes.Add(traducao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", traducao.IdiomaId);
            return View(traducao);
        }

        // GET: Traducao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var traducao = db.Traducoes.Find(id);
            if (traducao == null)
                return HttpNotFound();

            ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", traducao.IdiomaId);
            return View(traducao);
        }

        // POST: Traducao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Traducao traducao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(traducao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", traducao.IdiomaId);
            return View(traducao);
        }

        // GET: Traducao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var traducao = db.Traducoes.Include(t => t.Idioma).FirstOrDefault(t => t.Id == id);
            if (traducao == null)
                return HttpNotFound();

            return View(traducao);
        }

        // POST: Traducao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var traducao = db.Traducoes.Find(id);
            db.Traducoes.Remove(traducao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // (Opcional) GET: Traducao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var traducao = db.Traducoes.Include(t => t.Idioma).FirstOrDefault(t => t.Id == id);

            if (traducao == null)
                return HttpNotFound();

            return View(traducao);
        }
    }
}

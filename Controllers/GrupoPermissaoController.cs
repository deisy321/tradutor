using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tradutor.DAL;
using Tradutor.Models;

namespace Tradutor.Controllers
{
    public class GrupoPermissaoController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: GrupoPermissao
        public ActionResult Index()
        {
            return View(db.GruposPermissao.ToList());
        }

        // GET: GrupoPermissao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GrupoPermissao grupoPermissao = db.GruposPermissao
                .Include(g => g.Utilizadores)
                .FirstOrDefault(g => g.Id == id);

            if (grupoPermissao == null) return HttpNotFound();

            return View(grupoPermissao);
        }

        // GET: GrupoPermissao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrupoPermissao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Nome")] GrupoPermissao grupoPermissao)
        {
            if (ModelState.IsValid)
            {
                db.GruposPermissao.Add(grupoPermissao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grupoPermissao);
        }

        // GET: GrupoPermissao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GrupoPermissao grupoPermissao = db.GruposPermissao.Find(id);
            if (grupoPermissao == null) return HttpNotFound();

            return View(grupoPermissao);
        }

        // POST: GrupoPermissao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Nome")] GrupoPermissao grupoPermissao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupoPermissao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grupoPermissao);
        }

        // GET: GrupoPermissao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GrupoPermissao grupoPermissao = db.GruposPermissao.Find(id);
            if (grupoPermissao == null) return HttpNotFound();

            return View(grupoPermissao);
        }

        // POST: GrupoPermissao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GrupoPermissao grupoPermissao = db.GruposPermissao.Find(id);
            db.GruposPermissao.Remove(grupoPermissao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

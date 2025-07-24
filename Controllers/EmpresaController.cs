using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tradutor.Models;
using Tradutor.DAL;

namespace Tradutor.Controllers
{
    public class EmpresaController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Empresa
        public ActionResult Index()
        {
            return View(db.Empresas.ToList());
        }

        // GET: Empresa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empresa empresa = db.Empresas.Find(id);

            if (empresa == null)
                return HttpNotFound();

            return View(empresa);
        }

        // GET: Empresa/Create
        public ActionResult Create()
        {
            // Passa a lista de empresas para a View
            ViewBag.ListaEmpresas = db.Empresas.ToList();
            return View();
        }

        // POST: Empresa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Empresas.Add(empresa);
                db.SaveChanges();
                // Após salvar, redireciona para Create novamente para mostrar a lista atualizada
                return RedirectToAction("Create");
            }

            // Em caso de erro, retorna a lista para a View para continuar mostrando a tabela
            ViewBag.ListaEmpresas = db.Empresas.ToList();
            return View(empresa);
        }

        // GET: Empresa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empresa empresa = db.Empresas.Find(id);

            if (empresa == null)
                return HttpNotFound();

            return View(empresa);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empresa);
        }

        // GET: Empresa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empresa empresa = db.Empresas.Find(id);

            if (empresa == null)
                return HttpNotFound();

            return View(empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empresa empresa = db.Empresas.Find(id);
            db.Empresas.Remove(empresa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}

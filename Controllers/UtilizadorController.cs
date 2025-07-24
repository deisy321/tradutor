using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tradutor.DAL;
using Tradutor.Models;

namespace Tradutor.Controllers
{
    public class UtilizadorController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        // GET: Utilizador
        public ActionResult Index()
        {
            var utilizadores = db.Utilizadores
                .Include(u => u.GrupoPermissao)
                .Include(u => u.Empresas)
                .ToList();

            return View(utilizadores);
        }

        // GET: Utilizador/Create
        public ActionResult Create()
        {
            PreencherViewBags(null);
            return View(new Utilizador());
        }

        // POST: Utilizador/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Utilizador utilizador, int[] EmpresasSelecionadas)
        {
            if (ModelState.IsValid)
            {
                if (EmpresasSelecionadas != null && EmpresasSelecionadas.Any())
                {
                    utilizador.Empresas = new List<Empresa>();
                    foreach (var empresaId in EmpresasSelecionadas)
                    {
                        var empresa = db.Empresas.Find(empresaId);
                        if (empresa != null)
                            utilizador.Empresas.Add(empresa);
                    }
                }

                db.Utilizadores.Add(utilizador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PreencherViewBags(utilizador.GrupoPermissaoId, EmpresasSelecionadas);
            return View(utilizador);
        }

        // GET: Utilizador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var utilizador = db.Utilizadores
                .Include(u => u.GrupoPermissao)
                .Include(u => u.Empresas)
                .FirstOrDefault(u => u.Id == id);

            if (utilizador == null)
                return HttpNotFound();

            return View(utilizador);
        }

        // GET: Utilizador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var utilizador = db.Utilizadores
                .Include(u => u.Empresas)
                .FirstOrDefault(u => u.Id == id);

            if (utilizador == null)
                return HttpNotFound();

            var empresasSelecionadas = utilizador.Empresas.Select(e => e.Id).ToArray();
            PreencherViewBags(utilizador.GrupoPermissaoId, empresasSelecionadas);

            return View(utilizador);
        }

        // POST: Utilizador/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Utilizador utilizador, int[] EmpresasSelecionadas)
        {
            if (ModelState.IsValid)
            {
                var utilizadorExistente = db.Utilizadores
                    .Include(u => u.Empresas)
                    .FirstOrDefault(u => u.Id == utilizador.Id);

                if (utilizadorExistente == null)
                    return HttpNotFound();

                // Atualiza os dados principais
                utilizadorExistente.Nome = utilizador.Nome;
                utilizadorExistente.Email = utilizador.Email;
                utilizadorExistente.GrupoPermissaoId = utilizador.GrupoPermissaoId;

                // Atualiza as empresas associadas
                utilizadorExistente.Empresas.Clear();

                if (EmpresasSelecionadas != null && EmpresasSelecionadas.Any())
                {
                    foreach (var empresaId in EmpresasSelecionadas)
                    {
                        var empresa = db.Empresas.Find(empresaId);
                        if (empresa != null)
                            utilizadorExistente.Empresas.Add(empresa);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PreencherViewBags(utilizador.GrupoPermissaoId, EmpresasSelecionadas);
            return View(utilizador);
        }

        // GET: Utilizador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var utilizador = db.Utilizadores
                .Include(u => u.GrupoPermissao)
                .Include(u => u.Empresas)
                .FirstOrDefault(u => u.Id == id);

            if (utilizador == null)
                return HttpNotFound();

            return View(utilizador);
        }

        // POST: Utilizador/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var utilizador = db.Utilizadores
                .Include(u => u.Empresas)
                .FirstOrDefault(u => u.Id == id);

            if (utilizador == null)
                return HttpNotFound();

            // Remove os relacionamentos antes de deletar
            utilizador.Empresas.Clear();

            db.Utilizadores.Remove(utilizador);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Preenche os ViewBags usados para o formulário de Create e Edit
        /// </summary>
        private void PreencherViewBags(int? grupoPermissaoId, int[] empresasSelecionadas = null)
        {
            ViewBag.GrupoPermissaoId = new SelectList(db.GruposPermissao, "Id", "Nome", grupoPermissaoId);
            ViewBag.Empresas = new MultiSelectList(db.Empresas, "Id", "Nome", empresasSelecionadas);
        }
    }
}

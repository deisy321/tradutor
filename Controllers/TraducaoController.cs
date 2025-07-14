using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tradutor.DAL;
using Tradutor.Models;
using System.Collections.Generic;

namespace Tradutor.Controllers
{
    public class TraducaoController : Controller
    {
        // Declarar contexto do banco
        private readonly AppDbContext db = new AppDbContext();

        // GET: Traducao
        public ActionResult Index()
        {
            var traducoes = db.Traducoes.Include(t => t.Idioma)
                .OrderBy(t => t.Idioma.Descricao)
                .ThenBy(t => t.TextoOriginal)
                .ToList();
            return View(traducoes);
        }

        // GET: Traducao/Create?idiomaId=1
        public ActionResult Create(int? idiomaId)
        {
            if (idiomaId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", idiomaId);
            var traducao = new Traducao { IdiomaId = idiomaId.Value };
            return View(traducao);
        }

        // POST: Traducao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int IdiomaId, string TextoOriginal, string TextoTraduzido)
        {
            if (string.IsNullOrWhiteSpace(TextoOriginal))
            {
                ModelState.AddModelError("TextoOriginal", "Por favor, insira pelo menos uma palavra ou frase.");
            }

            if (string.IsNullOrWhiteSpace(TextoTraduzido))
            {
                ModelState.AddModelError("TextoTraduzido", "Por favor, insira as traduções correspondentes.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", IdiomaId);
                return View();
            }

            // Separar as linhas em arrays
            var linhasOriginais = TextoOriginal.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var linhasTraduzidas = TextoTraduzido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (linhasOriginais.Length != linhasTraduzidas.Length)
            {
                ModelState.AddModelError("", "O número de frases originais e traduções deve ser igual.");
                ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", IdiomaId);
                return View();
            }

            for (int i = 0; i < linhasOriginais.Length; i++)
            {
                var traducao = new Traducao
                {
                    IdiomaId = IdiomaId,
                    TextoOriginal = linhasOriginais[i].Trim(),
                    TextoTraduzido = linhasTraduzidas[i].Trim()
                };

                db.Traducoes.Add(traducao);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
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

        // GET: Traducao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var traducao = db.Traducoes.Include(t => t.Idioma).FirstOrDefault(t => t.Id == id);
            if (traducao == null)
                return HttpNotFound();

            return View(traducao);
        }

        // ------------------------------
        // NOVO MÉTODO: Traduz frase/palavra(s) usando o banco
        // Pode ser usado via Ajax ou diretamente em outra action
        // ------------------------------
        [HttpPost]
        public JsonResult TraduzirFrase(string frase, int idiomaId)
        {
            if (string.IsNullOrWhiteSpace(frase))
                return Json(new { sucesso = false, mensagem = "Frase vazia." });

            // Busca tradução da frase inteira
            var traducaoFrase = db.Traducoes
                .FirstOrDefault(t => t.TextoOriginal == frase && t.IdiomaId == idiomaId);

            if (traducaoFrase != null)
            {
                return Json(new { sucesso = true, traducao = traducaoFrase.TextoTraduzido });
            }

            // Se não achou frase inteira, traduz palavra por palavra
            var palavras = frase.Split(' ');
            List<string> palavrasTraduzidas = new List<string>();

            foreach (var palavra in palavras)
            {
                var traducaoPalavra = db.Traducoes
                    .FirstOrDefault(t => t.TextoOriginal == palavra && t.IdiomaId == idiomaId);

                palavrasTraduzidas.Add(traducaoPalavra != null ? traducaoPalavra.TextoTraduzido : palavra);
            }

            var resultado = string.Join(" ", palavrasTraduzidas);
            return Json(new { sucesso = true, traducao = resultado });
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

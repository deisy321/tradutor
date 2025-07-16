using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tradutor.DAL;
using Tradutor.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Tradutor.Controllers
{
    public class TraducaoController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        private void CarregarIdiomasNoViewBag()
        {
            ViewBag.IdiomasCompletos = db.Idiomas.OrderBy(i => i.Descricao).ToList();
        }

        // GET: Traducao
        public ActionResult Index()
        {
            var traducoes = db.Traducoes.Include(t => t.Idioma)
                .OrderBy(t => t.Idioma.Descricao)
                .ThenBy(t => t.TextoOriginal)
                .ToList();

            CarregarIdiomasNoViewBag();

            var idiomaSessao = Session["Idioma"]?.ToString().ToLower() ?? "pt";
            var idiomaAtual = db.Idiomas.FirstOrDefault(i => i.Codigo.ToLower() == idiomaSessao)
                              ?? db.Idiomas.OrderBy(i => i.Id).FirstOrDefault();

            ViewBag.IdiomaAtualId = idiomaAtual?.Id ?? 1;

            return View(traducoes);
        }

        // GET: Traducao/Create
        public ActionResult Create(int? idiomaId)
        {
            if (idiomaId == null)
            {
                var primeiroIdioma = db.Idiomas.OrderBy(i => i.Id).FirstOrDefault();
                if (primeiroIdioma != null)
                    idiomaId = primeiroIdioma.Id;
                else
                    return HttpNotFound("Nenhum idioma cadastrado.");
            }

            CarregarIdiomasNoViewBag();
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
                CarregarIdiomasNoViewBag();
                ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", IdiomaId);
                return View();
            }

            var linhasOriginais = TextoOriginal.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var linhasTraduzidas = TextoTraduzido.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (linhasOriginais.Length != linhasTraduzidas.Length)
            {
                ModelState.AddModelError("", "O número de frases originais e traduções deve ser igual.");
                CarregarIdiomasNoViewBag();
                ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", IdiomaId);
                return View();
            }

            for (int i = 0; i < linhasOriginais.Length; i++)
            {
                var original = linhasOriginais[i].Trim();
                var traduzido = linhasTraduzidas[i].Trim();

                bool jaExiste = db.Traducoes.Any(t =>
                    t.IdiomaId == IdiomaId &&
                    t.TextoOriginal.ToLower() == original.ToLower());

                if (!jaExiste)
                {
                    var novaTraducao = new Traducao
                    {
                        IdiomaId = IdiomaId,
                        TextoOriginal = original,
                        TextoTraduzido = traduzido
                    };

                    db.Traducoes.Add(novaTraducao);
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Traducoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Traducao traducao = db.Traducoes.Find(id);
            if (traducao == null)
                return HttpNotFound();

            ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", traducao.IdiomaId);

            return View(traducao);
        }

        // POST: Traducoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Traducao traducao)
        {
            if (ModelState.IsValid)
            {
                // Busca a tradução original para evitar conflitos
                var traducaoOriginal = db.Traducoes.Find(traducao.Id);
                if (traducaoOriginal == null)
                    return HttpNotFound();

                // Atualiza os campos editáveis
                traducaoOriginal.TextoOriginal = traducao.TextoOriginal;
                traducaoOriginal.TextoTraduzido = traducao.TextoTraduzido;
                traducaoOriginal.IdiomaId = traducao.IdiomaId;

                // Salva as alterações
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Recarrega dropdown se tiver erro para mostrar na view
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

            CarregarIdiomasNoViewBag();
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

            CarregarIdiomasNoViewBag();
            return View(traducao);
        }
        public ActionResult Gerenciar(int idiomaId, string busca = "")
        {
            var idioma = db.Idiomas.Find(idiomaId);
            if (idioma == null) return HttpNotFound();

            var traducoes = db.Traducoes
                              .Where(t => t.IdiomaId == idiomaId &&
                                          (string.IsNullOrEmpty(busca) ||
                                           t.TextoOriginal.Contains(busca) ||
                                           t.TextoTraduzido.Contains(busca)))
                              .OrderBy(t => t.TextoOriginal)
                              .ToList();

            ViewBag.Idioma = idioma;
            ViewBag.Busca = busca; // Para manter o texto preenchido no campo busca

            return View(traducoes);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirSelecionadas(int idiomaId, int[] idsSelecionados)
        {
            if (idsSelecionados != null && idsSelecionados.Length > 0)
            {
                var traducoes = db.Traducoes
                    .Where(t => idsSelecionados.Contains(t.Id) && t.IdiomaId == idiomaId)
                    .ToList();

                db.Traducoes.RemoveRange(traducoes);
                db.SaveChanges();
            }

            return RedirectToAction("Gerenciar", new { idiomaId = idiomaId });
        }

        // Tradução rápida via Ajax
        [HttpPost]
        public JsonResult TraduzirFrase(string frase, int idiomaId)
        {
            if (string.IsNullOrWhiteSpace(frase))
                return Json(new { sucesso = false, mensagem = "Frase vazia." });

            var fraseNormalizada = frase.Trim().ToLower();

            var traducaoFrase = db.Traducoes
                .FirstOrDefault(t => t.IdiomaId == idiomaId && t.TextoOriginal.ToLower() == fraseNormalizada);

            if (traducaoFrase != null)
            {
                return Json(new { sucesso = true, traducao = traducaoFrase.TextoTraduzido });
            }

            var palavras = Regex.Matches(frase, @"\b[\w']+\b")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            List<string> palavrasTraduzidas = new List<string>();

            foreach (var palavra in palavras)
            {
                var palavraNorm = palavra.ToLower();

                var traducaoPalavra = db.Traducoes
                    .FirstOrDefault(t => t.IdiomaId == idiomaId && t.TextoOriginal.ToLower() == palavraNorm);

                palavrasTraduzidas.Add(traducaoPalavra != null ? traducaoPalavra.TextoTraduzido : palavra);
            }

            var fraseTraduzida = string.Join(" ", palavrasTraduzidas);

            return Json(new { sucesso = true, traducao = fraseTraduzida });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Tradutor.DAL;
using Tradutor.Models;

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
                ModelState.AddModelError("TextoTraduzido", "Por favor, insira as tradu��es correspondentes.");
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
                ModelState.AddModelError("", "O n�mero de frases originais e tradu��es deve ser igual.");
                CarregarIdiomasNoViewBag();
                ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", IdiomaId);
                return View();
            }

            bool encontrouDuplicado = false;

            for (int i = 0; i < linhasOriginais.Length; i++)
            {
                var original = linhasOriginais[i].Trim();
                var traduzido = linhasTraduzidas[i].Trim();

                bool jaExiste = db.Traducoes.Any(t =>
                    t.IdiomaId == IdiomaId &&
                    t.TextoOriginal.ToLower() == original.ToLower());

                if (jaExiste)
                {
                    ModelState.AddModelError("TextoOriginal", $"A palavra ou frase '{original}' j� existe para esse idioma.");
                    encontrouDuplicado = true;
                }
                else
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

            if (encontrouDuplicado)
            {
                CarregarIdiomasNoViewBag();
                ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", IdiomaId);
                return View();
            }

            db.SaveChanges();

            return RedirectToAction("Gerenciar", new { idiomaId = IdiomaId });
        }
        [HttpPost]
        public ActionResult ExcluirSelecionadas(int[] idsSelecionados)
        {
            if (idsSelecionados != null && idsSelecionados.Length > 0)
            {
                foreach (var id in idsSelecionados)
                {
                    var traducao = db.Traducoes.Find(id);
                    if (traducao != null)
                    {
                        db.Traducoes.Remove(traducao);
                    }
                }

                db.SaveChanges();
            }

            // Para redirecionar para o idioma da primeira tradu��o exclu�da, se quiser
            return RedirectToAction("Gerenciar", new { idiomaId = idsSelecionados != null && idsSelecionados.Length > 0 ? db.Traducoes.Find(idsSelecionados.FirstOrDefault())?.IdiomaId ?? 1 : 1 });
        }
        // GET: Traducao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var traducao = db.Traducoes.Find(id);
            if (traducao == null)
                return HttpNotFound();

            CarregarIdiomasNoViewBag();
            ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", traducao.IdiomaId);

            return View(traducao);
        }

        // POST: Traducao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Traducao model)
        {
            if (!ModelState.IsValid)
            {
                CarregarIdiomasNoViewBag();
                ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", model.IdiomaId);
                return View(model);
            }

            // Verifica se j� existe uma tradu��o duplicada para o mesmo idioma e texto original
            bool existeDuplicado = db.Traducoes.Any(t =>
                t.Id != model.Id &&
                t.IdiomaId == model.IdiomaId &&
                t.TextoOriginal.ToLower() == model.TextoOriginal.ToLower());

            if (existeDuplicado)
            {
                ModelState.AddModelError("TextoOriginal", "J� existe uma tradu��o para essa palavra/frase neste idioma.");
                CarregarIdiomasNoViewBag();
                ViewBag.IdiomaId = new SelectList(db.Idiomas, "Id", "Descricao", model.IdiomaId);
                return View(model);
            }

            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Gerenciar", new { idiomaId = model.IdiomaId });
        }



        // GET: Traducao/Gerenciar
        public ActionResult Gerenciar(int idiomaId)
        {
            var idioma = db.Idiomas.Find(idiomaId);
            if (idioma == null)
            {
                return HttpNotFound("Idioma n�o encontrado.");
            }

            var traducoes = db.Traducoes
                .Where(t => t.IdiomaId == idiomaId)
                .OrderBy(t => t.TextoOriginal)
                .ToList();

            // Passa o objeto idioma para a ViewBag para evitar NullReferenceException na view
            ViewBag.Idioma = idioma;

            return View(traducoes);
        }

        // Tradu��o r�pida via Ajax
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

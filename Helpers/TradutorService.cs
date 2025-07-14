using System.Linq;
using Tradutor.DAL;  // ajuste para o namespace do seu contexto
using System.Collections.Generic;

namespace Tradutor.Services
{
    public class TradutorService
    {
        private readonly AppDbContext _db;

        public TradutorService()
        {
            _db = new AppDbContext();
        }

        public string TraduzirFrase(string fraseOriginal, int idiomaId)
        {
            // Tenta traduzir a frase inteira
            var traducaoFrase = _db.Traducoes
                .FirstOrDefault(t => t.TextoOriginal == fraseOriginal && t.IdiomaId == idiomaId);

            if (traducaoFrase != null)
                return traducaoFrase.TextoTraduzido;

            // Se não encontrar a frase, tenta traduzir palavra por palavra
            var palavras = fraseOriginal.Split(' ');

            List<string> palavrasTraduzidas = new List<string>();

            foreach (var palavra in palavras)
            {
                var traducaoPalavra = _db.Traducoes
                    .FirstOrDefault(t => t.TextoOriginal == palavra && t.IdiomaId == idiomaId);

                palavrasTraduzidas.Add(traducaoPalavra != null ? traducaoPalavra.TextoTraduzido : palavra);
            }

            return string.Join(" ", palavrasTraduzidas);
        }
    }
}

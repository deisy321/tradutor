using System.Linq;
using System.Web;
using Tradutor.DAL;

namespace Tradutor.Helpers
{
    public static class TradutorHelper
    {
        // Usa idioma da sessão
        public static string Traduzir(string textoOriginal)
        {
            string idiomaCodigo = HttpContext.Current.Session?["Idioma"]?.ToString() ?? "pt";
            return Traduzir(textoOriginal, idiomaCodigo);
        }

        // Usa idioma passado como parâmetro
        public static string Traduzir(string textoOriginal, string idiomaCodigo)
        {
            if (string.IsNullOrWhiteSpace(textoOriginal))
                return textoOriginal;

            using (var db = new AppDbContext())
            {
                var palavras = textoOriginal.Split(' ');

                for (int i = 0; i < palavras.Length; i++)
                {
                    var palavra = palavras[i].Trim().ToLower();

                    var traducao = db.Traducoes
                        .FirstOrDefault(t =>
                            t.TextoOriginal.ToLower() == palavra &&
                            t.Idioma.Codigo == idiomaCodigo);

                    if (traducao != null)
                        palavras[i] = traducao.TextoTraduzido;
                }

                return string.Join(" ", palavras);
            }
        }
    }
}

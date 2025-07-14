using System.Linq;
using System.Web;
using Tradutor.DAL;
using Tradutor.Models;

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
            using (var db = new AppDbContext())
            {
                var traducao = db.Traducoes
                    .FirstOrDefault(t => t.TextoOriginal == textoOriginal && t.Idioma.Codigo == idiomaCodigo);

                if (traducao != null)
                    return traducao.TextoTraduzido;

                // 🔴 NÃO faz chamada à API externa. Apenas retorna o original.
                return textoOriginal;
            }
        }
    }
}

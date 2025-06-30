using System.Threading.Tasks;

namespace Tradutor.Helpers
{
    public static class TradutorHelper
    {
        public static string Traduzir(string texto, string idioma)
        {
            return Task.Run(() => TradutorService.TraduzirTexto(texto, idioma)).Result;
        }
    }
}

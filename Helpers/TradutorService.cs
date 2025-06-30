using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Tradutor.Helpers
{
    public static class TradutorService
    {
        public static async Task<string> TraduzirTexto(string texto, string idiomaDestino)
        {
            string cacheKey = $"traducao_{texto}_{idiomaDestino}";
            var cache = HttpRuntime.Cache;

            if (cache[cacheKey] != null)
                return (string)cache[cacheKey];

            using (var client = new HttpClient())
            {
                string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={idiomaDestino}&dt=t&q={HttpUtility.UrlEncode(texto)}";

                var response = await client.GetStringAsync(url);

                // A resposta é algo como: [[[\"Hello\",\"Olá\",...]]]
                var traducao = response.Split('"')[1];

                cache.Insert(cacheKey, traducao, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);

                return traducao;
            }
        }
    }
}

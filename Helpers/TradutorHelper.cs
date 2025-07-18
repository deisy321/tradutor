using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                // Normaliza texto para minúsculas para comparação
                string textoMinusculo = textoOriginal.ToLowerInvariant();

                // Trazer traduções do idioma para memória
                var traducoesIdioma = db.Traducoes
                    .Where(t => t.Idioma.Codigo == idiomaCodigo)
                    .ToList();

                // Tenta traduzir a frase inteira (em memória)
                var traducaoFrase = traducoesIdioma
                    .FirstOrDefault(t => t.TextoOriginal.ToLowerInvariant() == textoMinusculo);

                if (traducaoFrase != null)
                {
                    return CapitalizarPrimeiraLetra(traducaoFrase.TextoTraduzido);
                }

                // Se não encontrou a frase, traduz palavra por palavra
                var tokens = Regex.Matches(textoOriginal, @"\w+|[^\w\s]", RegexOptions.Compiled);

                var resultado = tokens.Cast<Match>().Select(token =>
                {
                    if (Regex.IsMatch(token.Value, @"^\w+$"))
                    {
                        string palavraMinuscula = token.Value.ToLowerInvariant();

                        var traducao = traducoesIdioma
                            .FirstOrDefault(t => t.TextoOriginal.ToLowerInvariant() == palavraMinuscula);

                        if (traducao != null)
                        {
                            return PreservarCapitalizacao(token.Value, traducao.TextoTraduzido);
                        }
                    }

                    return token.Value;
                });

                return string.Join("", AjustarEspacos(resultado));
            }
        }

        private static string[] AjustarEspacos(IEnumerable<string> tokens)
        {
            var lista = tokens.ToList();
            var resultado = new List<string>();

            for (int i = 0; i < lista.Count; i++)
            {
                var token = lista[i];
                resultado.Add(token);

                if (i < lista.Count - 1 && !Regex.IsMatch(lista[i + 1], @"^\p{P}$"))
                {
                    resultado.Add(" ");
                }
            }

            return resultado.ToArray();
        }

        private static string PreservarCapitalizacao(string original, string traduzido)
        {
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(traduzido))
                return traduzido;

            if (original.All(char.IsUpper))
                return traduzido.ToUpperInvariant();

            if (char.IsUpper(original[0]))
                return CapitalizarPrimeiraLetra(traduzido);

            return traduzido.ToLowerInvariant();
        }

        private static string CapitalizarPrimeiraLetra(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            if (texto.Length == 1)
                return texto.ToUpperInvariant();

            return char.ToUpperInvariant(texto[0]) + texto.Substring(1);
        }
    }
}

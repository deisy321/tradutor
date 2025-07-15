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
                // Normaliza texto para minúsculas para consulta
                string textoMinusculo = textoOriginal.ToLower();

                // Tenta traduzir a frase inteira
                var traducaoFrase = db.Traducoes
                    .FirstOrDefault(t =>
                        t.TextoOriginal.ToLower() == textoMinusculo &&
                        t.Idioma.Codigo == idiomaCodigo);

                if (traducaoFrase != null)
                {
                    // Retorna a frase traduzida preservando a capitalização original (inicial maiúscula)
                    return CapitalizarPrimeiraLetra(traducaoFrase.TextoTraduzido);
                }

                // Se não encontrou a frase, traduz palavra por palavra
                // Regex para separar palavras e pontuação
                var tokens = Regex.Matches(textoOriginal, @"\w+|[^\w\s]", RegexOptions.Compiled);

                var resultado = tokens.Cast<Match>().Select(token =>
                {
                    // Se for palavra (letras/dígitos)
                    if (Regex.IsMatch(token.Value, @"^\w+$"))
                    {
                        string palavraMinuscula = token.Value.ToLower();

                        var traducao = db.Traducoes
                            .FirstOrDefault(t =>
                                t.TextoOriginal.ToLower() == palavraMinuscula &&
                                t.Idioma.Codigo == idiomaCodigo);

                        if (traducao != null)
                        {
                            // Preserva capitalização da palavra original
                            return PreservarCapitalizacao(token.Value, traducao.TextoTraduzido);
                        }
                    }

                    // Se não for palavra ou não tem tradução, retorna o token original
                    return token.Value;
                });

                // Junta tokens traduzidos mantendo espaços entre palavras
                // Essa regex trata a pontuação para não adicionar espaço antes dela
                return string.Join("", AjustarEspacos(resultado));
            }
        }

        // Ajusta espaços entre tokens, não adiciona espaço antes de pontuação
        private static string[] AjustarEspacos(IEnumerable<string> tokens)
        {
            var lista = tokens.ToList();
            var resultado = new System.Collections.Generic.List<string>();

            for (int i = 0; i < lista.Count; i++)
            {
                var token = lista[i];
                resultado.Add(token);

                // Adiciona espaço se o próximo token existir e o próximo token não for pontuação
                if (i < lista.Count - 1 && !Regex.IsMatch(lista[i + 1], @"^\p{P}$"))
                {
                    resultado.Add(" ");
                }
            }

            return resultado.ToArray();
        }

        // Preserva a capitalização da palavra original na tradução
        private static string PreservarCapitalizacao(string original, string traduzido)
        {
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(traduzido))
                return traduzido;

            // Se original toda maiúscula, deixa traduzido todo maiúsculo
            if (original.All(char.IsUpper))
                return traduzido.ToUpper();

            // Se original só a primeira letra maiúscula, aplica isso na tradução
            if (char.IsUpper(original[0]))
                return CapitalizarPrimeiraLetra(traduzido);

            // Senão, retorna a tradução em minúsculas
            return traduzido.ToLower();
        }

        // Capitaliza a primeira letra da string
        private static string CapitalizarPrimeiraLetra(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            if (texto.Length == 1)
                return texto.ToUpper();

            return char.ToUpper(texto[0]) + texto.Substring(1);
        }
    }
}

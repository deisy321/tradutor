namespace Tradutor.Models
{
    public class Idioma
    {
        public int Id { get; set; }
        public string Codigo { get; set; } // Ex: "pt", "en"
        public string Descricao { get; set; } // Ex: "Português", "Inglês"
        public string BandeiraUrl { get; set; } // Ex: https://example.com/pt.png
    }

}

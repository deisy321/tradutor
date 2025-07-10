namespace Tradutor.Models
{
    public class Idioma
    {
        public int Id { get; set; }
        public string Codigo { get; set; } // Ex: "pt", "en"
        public string Descricao { get; set; } // Ex: "Portugu�s", "Ingl�s"
        public string BandeiraUrl { get; set; } // Ex: https://example.com/pt.png
    }

}

namespace Tradutor.Models
{
    public class Traducao
    {
        public int Id { get; set; }

        public string TextoOriginal { get; set; }

        public string TextoTraduzido { get; set; }

        // FK para Idioma
        public int IdiomaId { get; set; }

        // Propriedade de navega��o para carregar o idioma associado
        public virtual Idioma Idioma { get; set; }
    }
}
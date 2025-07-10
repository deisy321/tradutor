namespace Tradutor.Models
{
    public class Traducao
    {
        public int Id { get; set; }

        public int IdiomaId { get; set; }           // FK para o idioma
        public string TextoOriginal { get; set; }   // texto em PT (ou texto base)
        public string TextoTraduzido { get; set; }  // texto traduzido no idioma

        public virtual Idioma Idioma { get; set; }  // relacionamento
    }
}

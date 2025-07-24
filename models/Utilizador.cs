using System.Collections.Generic;

namespace Tradutor.Models
{
    public class Utilizador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string Email { get; set; }

        public int GrupoPermissaoId { get; set; }
        public virtual GrupoPermissao GrupoPermissao { get; set; }

        // coleção de empresas - muitos para muitos
        public virtual ICollection<Empresa> Empresas { get; set; } = new HashSet<Empresa>();
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tradutor.Models
{
    public class GrupoPermissao
    {
        public int Id { get; set; }

        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Nome { get; set; }

        public virtual ICollection<Utilizador> Utilizadores { get; set; }
        public virtual ICollection<Empresa> Empresas { get; set; } = new HashSet<Empresa>();
    }
}

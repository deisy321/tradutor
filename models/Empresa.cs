using System.Collections.Generic;

namespace Tradutor.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Endereco { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string CNPJ { get; set; }

        public string Descricao { get; set; }

        public string Site { get; set; }

        public string Responsavel { get; set; }

      
        public virtual ICollection<Utilizador> Utilizadores { get; set; }
    }

}

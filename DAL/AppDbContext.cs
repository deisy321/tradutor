using Tradutor.Models;
using System.Data.Entity;

namespace Tradutor.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection") { }

        public DbSet<Idioma> Idiomas { get; set; }
        public DbSet<Traducao> Traducoes { get; set; }

        // Outros DbSets do projeto podem ser adicionados aqui, ex:
        // public DbSet<Traducao> Traducoes { get; set; }
    }
}

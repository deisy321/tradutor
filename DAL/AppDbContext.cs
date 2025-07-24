using Tradutor.Models;
using System.Data.Entity;

namespace Tradutor.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection") { }

        public DbSet<Idioma> Idiomas { get; set; }
        public DbSet<Traducao> Traducoes { get; set; }

        public DbSet<GrupoPermissao> GruposPermissao { get; set; }
        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Empresa> Empresas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar a relação muitos-para-muitos entre Utilizador e Empresa
            modelBuilder.Entity<Utilizador>()
                .HasMany(u => u.Empresas)
                .WithMany(e => e.Utilizadores)
                .Map(m =>
                {
                    m.ToTable("UtilizadorEmpresas"); // Nome da tabela associativa
                    m.MapLeftKey("UtilizadorId");
                    m.MapRightKey("EmpresaId");
                });
        }
    }
}

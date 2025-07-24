namespace tradutor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizarModelo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Utilizadors", "GrupoPermissaoId", "dbo.GrupoPermissaos");
            DropIndex("dbo.Utilizadors", new[] { "GrupoPermissaoId" });
            CreateTable(
                "dbo.Empresas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UtilizadorEmpresas",
                c => new
                    {
                        UtilizadorId = c.Int(nullable: false),
                        EmpresaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UtilizadorId, t.EmpresaId })
                .ForeignKey("dbo.Utilizadors", t => t.UtilizadorId, cascadeDelete: true)
                .ForeignKey("dbo.Empresas", t => t.EmpresaId, cascadeDelete: true)
                .Index(t => t.UtilizadorId)
                .Index(t => t.EmpresaId);
            
            AddColumn("dbo.Utilizadors", "Codigo", c => c.String());
            AddColumn("dbo.Utilizadors", "Email", c => c.String());
            AlterColumn("dbo.Utilizadors", "Nome", c => c.String());
            AlterColumn("dbo.Utilizadors", "GrupoPermissaoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Utilizadors", "GrupoPermissaoId");
            AddForeignKey("dbo.Utilizadors", "GrupoPermissaoId", "dbo.GrupoPermissaos", "Id", cascadeDelete: true);
            DropColumn("dbo.Utilizadors", "Empresa");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Utilizadors", "Empresa", c => c.String());
            DropForeignKey("dbo.Utilizadors", "GrupoPermissaoId", "dbo.GrupoPermissaos");
            DropForeignKey("dbo.UtilizadorEmpresas", "EmpresaId", "dbo.Empresas");
            DropForeignKey("dbo.UtilizadorEmpresas", "UtilizadorId", "dbo.Utilizadors");
            DropIndex("dbo.UtilizadorEmpresas", new[] { "EmpresaId" });
            DropIndex("dbo.UtilizadorEmpresas", new[] { "UtilizadorId" });
            DropIndex("dbo.Utilizadors", new[] { "GrupoPermissaoId" });
            AlterColumn("dbo.Utilizadors", "GrupoPermissaoId", c => c.Int());
            AlterColumn("dbo.Utilizadors", "Nome", c => c.String(nullable: false));
            DropColumn("dbo.Utilizadors", "Email");
            DropColumn("dbo.Utilizadors", "Codigo");
            DropTable("dbo.UtilizadorEmpresas");
            DropTable("dbo.Empresas");
            CreateIndex("dbo.Utilizadors", "GrupoPermissaoId");
            AddForeignKey("dbo.Utilizadors", "GrupoPermissaoId", "dbo.GrupoPermissaos", "Id");
        }
    }
}

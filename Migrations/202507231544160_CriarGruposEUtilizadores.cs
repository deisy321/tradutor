namespace tradutor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriarGruposEUtilizadores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GrupoPermissaos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(nullable: false),
                        Nome = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Utilizadors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Empresa = c.String(),
                        GrupoPermissaoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoPermissaos", t => t.GrupoPermissaoId)
                .Index(t => t.GrupoPermissaoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Utilizadors", "GrupoPermissaoId", "dbo.GrupoPermissaos");
            DropIndex("dbo.Utilizadors", new[] { "GrupoPermissaoId" });
            DropTable("dbo.Utilizadors");
            DropTable("dbo.GrupoPermissaos");
        }
    }
}

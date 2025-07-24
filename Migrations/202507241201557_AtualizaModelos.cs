namespace tradutor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizaModelos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "GrupoPermissao_Id", c => c.Int());
            CreateIndex("dbo.Empresas", "GrupoPermissao_Id");
            AddForeignKey("dbo.Empresas", "GrupoPermissao_Id", "dbo.GrupoPermissaos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Empresas", "GrupoPermissao_Id", "dbo.GrupoPermissaos");
            DropIndex("dbo.Empresas", new[] { "GrupoPermissao_Id" });
            DropColumn("dbo.Empresas", "GrupoPermissao_Id");
        }
    }
}

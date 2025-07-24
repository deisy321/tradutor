namespace tradutor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizacaoModelo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "Endereco", c => c.String());
            AddColumn("dbo.Empresas", "Telefone", c => c.String());
            AddColumn("dbo.Empresas", "Email", c => c.String());
            AddColumn("dbo.Empresas", "CNPJ", c => c.String());
            AddColumn("dbo.Empresas", "Descricao", c => c.String());
            AddColumn("dbo.Empresas", "Site", c => c.String());
            AddColumn("dbo.Empresas", "Responsavel", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Empresas", "Responsavel");
            DropColumn("dbo.Empresas", "Site");
            DropColumn("dbo.Empresas", "Descricao");
            DropColumn("dbo.Empresas", "CNPJ");
            DropColumn("dbo.Empresas", "Email");
            DropColumn("dbo.Empresas", "Telefone");
            DropColumn("dbo.Empresas", "Endereco");
        }
    }
}

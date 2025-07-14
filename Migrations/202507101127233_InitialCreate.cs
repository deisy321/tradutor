namespace tradutor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Idiomas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(),
                        Descricao = c.String(),
                        BandeiraUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Traducaos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdiomaId = c.Int(nullable: false),
                        TextoOriginal = c.String(),
                        TextoTraduzido = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Idiomas", t => t.IdiomaId, cascadeDelete: true)
                .Index(t => t.IdiomaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Traducaos", "IdiomaId", "dbo.Idiomas");
            DropIndex("dbo.Traducaos", new[] { "IdiomaId" });
            DropTable("dbo.Traducaos");
            DropTable("dbo.Idiomas");
        }
    }
}

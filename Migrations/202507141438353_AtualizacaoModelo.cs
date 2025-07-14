namespace tradutor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizacaoModelo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Traducaos", "TempoVerbal");
            DropColumn("dbo.Traducaos", "Genero");
            DropColumn("dbo.Traducaos", "Numero");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Traducaos", "Numero", c => c.String());
            AddColumn("dbo.Traducaos", "Genero", c => c.String());
            AddColumn("dbo.Traducaos", "TempoVerbal", c => c.String());
        }
    }
}

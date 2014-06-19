namespace Chowlog.Web.DataContexts.ChowlogMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtensionField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plates", "Extension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plates", "Extension");
        }
    }
}

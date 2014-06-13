namespace Chowlog.Web.DataContexts.ChowlogMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdToPlate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plates", "UserId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plates", "UserId");
        }
    }
}

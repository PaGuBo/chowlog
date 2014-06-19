namespace Chowlog.Web.DataContexts.ChowlogMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Plates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        TimeEaten = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                        HasPicture = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Plates");
        }
    }
}

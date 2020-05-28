namespace Veterinaria.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignPet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pets", "OwnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pets", "OwnerId");
            AddForeignKey("dbo.Pets", "OwnerId", "dbo.Owners", "id", cascadeDelete: true);
            DropColumn("dbo.Pets", "Owner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pets", "Owner", c => c.String());
            DropForeignKey("dbo.Pets", "OwnerId", "dbo.Owners");
            DropIndex("dbo.Pets", new[] { "OwnerId" });
            DropColumn("dbo.Pets", "OwnerId");
        }
    }
}

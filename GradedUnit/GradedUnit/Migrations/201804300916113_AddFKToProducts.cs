namespace GradedUnit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKToProducts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblProducts", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.tblProducts", "CategoryId");
            AddForeignKey("dbo.tblProducts", "CategoryId", "dbo.tblCatagories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblProducts", "CategoryId", "dbo.tblCatagories");
            DropIndex("dbo.tblProducts", new[] { "CategoryId" });
            DropColumn("dbo.tblProducts", "CategoryId");
        }
    }
}

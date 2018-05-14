namespace GradedUnit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKToOrderDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblOrderDetails", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.tblOrderDetails", "ProductId");
            AddForeignKey("dbo.tblOrderDetails", "ProductId", "dbo.tblProducts", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblOrderDetails", "ProductId", "dbo.tblProducts");
            DropIndex("dbo.tblOrderDetails", new[] { "ProductId" });
            DropColumn("dbo.tblOrderDetails", "ProductId");
        }
    }
}

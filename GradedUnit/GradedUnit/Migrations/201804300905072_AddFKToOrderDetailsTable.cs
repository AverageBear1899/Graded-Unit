namespace GradedUnit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKToOrderDetailsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblOrderDetails", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.tblOrderDetails", "OrderId");
            AddForeignKey("dbo.tblOrderDetails", "OrderId", "dbo.tblOrders", "OrderId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblOrderDetails", "OrderId", "dbo.tblOrders");
            DropIndex("dbo.tblOrderDetails", new[] { "OrderId" });
            DropColumn("dbo.tblOrderDetails", "OrderId");
        }
    }
}

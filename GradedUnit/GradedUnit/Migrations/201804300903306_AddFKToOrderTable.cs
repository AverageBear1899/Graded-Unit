namespace GradedUnit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKToOrderTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblOrders", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.tblOrders", "UserId");
            AddForeignKey("dbo.tblOrders", "UserId", "dbo.tblUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblOrders", "UserId", "dbo.tblUsers");
            DropIndex("dbo.tblOrders", new[] { "UserId" });
            DropColumn("dbo.tblOrders", "UserId");
        }
    }
}

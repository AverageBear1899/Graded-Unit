namespace GradedUnit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableUserRoles2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblUserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblUserRoles");
        }
    }
}

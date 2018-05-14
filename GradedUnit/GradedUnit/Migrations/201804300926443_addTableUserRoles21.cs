namespace GradedUnit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableUserRoles21 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.tblUserRoles");
            AddColumn("dbo.tblUserRoles", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.tblUserRoles", "RoleId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.tblUserRoles", new[] { "UserId", "RoleId" });
            CreateIndex("dbo.tblUserRoles", "UserId");
            CreateIndex("dbo.tblUserRoles", "RoleId");
            AddForeignKey("dbo.tblUserRoles", "RoleId", "dbo.tblRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.tblUserRoles", "UserId", "dbo.tblUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.tblUserRoles", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblUserRoles", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.tblUserRoles", "UserId", "dbo.tblUsers");
            DropForeignKey("dbo.tblUserRoles", "RoleId", "dbo.tblRoles");
            DropIndex("dbo.tblUserRoles", new[] { "RoleId" });
            DropIndex("dbo.tblUserRoles", new[] { "UserId" });
            DropPrimaryKey("dbo.tblUserRoles");
            DropColumn("dbo.tblUserRoles", "RoleId");
            DropColumn("dbo.tblUserRoles", "UserId");
            AddPrimaryKey("dbo.tblUserRoles", "Id");
        }
    }
}

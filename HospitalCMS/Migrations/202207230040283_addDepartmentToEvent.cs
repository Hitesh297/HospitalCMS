namespace HospitalCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDepartmentToEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Events", "DepartmentId");
            AddForeignKey("dbo.Events", "DepartmentId", "dbo.Departments", "DepartmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Events", new[] { "DepartmentId" });
            DropColumn("dbo.Events", "DepartmentId");
        }
    }
}

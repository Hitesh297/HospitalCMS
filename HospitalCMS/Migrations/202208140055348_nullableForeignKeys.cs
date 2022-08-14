namespace HospitalCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Comments", "PatientId", "dbo.Patients");
            DropIndex("dbo.Comments", new[] { "DoctorId" });
            DropIndex("dbo.Comments", new[] { "PatientId" });
            AlterColumn("dbo.Comments", "DoctorId", c => c.Int());
            AlterColumn("dbo.Comments", "PatientId", c => c.Int());
            CreateIndex("dbo.Comments", "DoctorId");
            CreateIndex("dbo.Comments", "PatientId");
            AddForeignKey("dbo.Comments", "DoctorId", "dbo.Doctors", "DoctorId");
            AddForeignKey("dbo.Comments", "PatientId", "dbo.Patients", "PatientId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Comments", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Comments", new[] { "PatientId" });
            DropIndex("dbo.Comments", new[] { "DoctorId" });
            AlterColumn("dbo.Comments", "PatientId", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "DoctorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "PatientId");
            CreateIndex("dbo.Comments", "DoctorId");
            AddForeignKey("dbo.Comments", "PatientId", "dbo.Patients", "PatientId", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "DoctorId", "dbo.Doctors", "DoctorId", cascadeDelete: true);
        }
    }
}

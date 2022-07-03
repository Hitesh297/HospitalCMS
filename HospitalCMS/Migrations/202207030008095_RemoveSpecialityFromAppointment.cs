namespace HospitalCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSpecialityFromAppointment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "SpecialityId", "dbo.Specialities");
            DropIndex("dbo.Appointments", new[] { "SpecialityId" });
            DropColumn("dbo.Appointments", "SpecialityId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "SpecialityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Appointments", "SpecialityId");
            AddForeignKey("dbo.Appointments", "SpecialityId", "dbo.Specialities", "SpecialityId", cascadeDelete: true);
        }
    }
}

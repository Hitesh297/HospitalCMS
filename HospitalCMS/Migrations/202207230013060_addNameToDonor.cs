namespace HospitalCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNameToDonor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donors", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Donors", "Name");
        }
    }
}

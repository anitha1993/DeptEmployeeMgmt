namespace DeptEmpMgmt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LoginCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LoginCount");
        }
    }
}

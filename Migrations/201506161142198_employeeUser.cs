namespace DeptEmpMgmt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employeeUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "UserInfo_UserInfoId", "dbo.UserInfoes");
            DropIndex("dbo.AspNetUsers", new[] { "UserInfo_UserInfoId" });
            AddColumn("dbo.Employees", "Email", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Employee_EmployeeId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Employee_EmployeeId");
            AddForeignKey("dbo.AspNetUsers", "Employee_EmployeeId", "dbo.Employees", "EmployeeId");
            DropColumn("dbo.AspNetUsers", "UserInfo_UserInfoId");
            DropTable("dbo.UserInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        UserInfoId = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserInfoId);
            
            AddColumn("dbo.AspNetUsers", "UserInfo_UserInfoId", c => c.Int());
            DropForeignKey("dbo.AspNetUsers", "Employee_EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "Employee_EmployeeId" });
            DropColumn("dbo.AspNetUsers", "Employee_EmployeeId");
            DropColumn("dbo.Employees", "Email");
            CreateIndex("dbo.AspNetUsers", "UserInfo_UserInfoId");
            AddForeignKey("dbo.AspNetUsers", "UserInfo_UserInfoId", "dbo.UserInfoes", "UserInfoId");
        }
    }
}

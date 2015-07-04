namespace DeptEmpMgmt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Department : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "Departments_DepartmentId", newName: "Department_DepartmentId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Departments_DepartmentId", newName: "IX_Department_DepartmentId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Department_DepartmentId", newName: "IX_Departments_DepartmentId");
            RenameColumn(table: "dbo.AspNetUsers", name: "Department_DepartmentId", newName: "Departments_DepartmentId");
        }
    }
}

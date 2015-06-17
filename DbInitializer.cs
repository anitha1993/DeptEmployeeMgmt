using DeptEmpMgmt.Migrations;
using DeptEmpMgmt.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeptEmpMgmt
{
     class DbInitializer : MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>
    {

        ApplicationDbContext context = new ApplicationDbContext();

       
       
    }
        
    
    
}

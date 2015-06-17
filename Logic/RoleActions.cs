using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DeptEmpMgmt.Models;

namespace DeptEmpMgmt
{
    class RoleActions
    {
        internal void AddUserAndRole()
        {

            ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleMgr = new RoleManager<IdentityRole>(roleStore);
            if (!roleMgr.RoleExists("Admin"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "Admin" });
            }

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser
            {

                Email = "admin@admin.com"
            };
            IdUserResult = userMgr.Create(appUser, "adminA123...");

            
            if (!userMgr.IsInRole(userMgr.FindByEmail("admin@admin.com").Id, "Admin"))
            {
                IdUserResult = userMgr.AddToRole(userMgr.FindByEmail("admin@admin.com").Id, "canEdit");
            }
        }
    }
}

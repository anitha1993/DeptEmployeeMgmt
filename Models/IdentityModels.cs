using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
//using DeptEmpMgmt.Migrations;
using DeptEmpMgmt.Models;
using System.ComponentModel.DataAnnotations.Schema;
namespace DeptEmpMgmt.Models
{

    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("DepartmentId")]
        public int DepartmentId;
        public virtual Department Departments { get; set; }
        public string RandomPassword { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
           // Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());

        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        // public DbSet<UserInfo> UserInfo { get; set; }

        public static void Initialize()
        {
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new
                                     RoleStore<IdentityRole>(new ApplicationDbContext()));




            string name = "Admin";
            string password = "123456";


            //Create Role Admin if it does not exist
            if (!RoleManager.RoleExists(name))
            {
                var roleresult = RoleManager.Create(new IdentityRole(name));
            }

            //Create User=Admin with password=123456
            var user = new ApplicationUser();
            user.UserName = name;

            var adminresult = UserManager.Create(user, password);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, name);
            }
        }
        public static ApplicationDbContext Create()
        {

            return new ApplicationDbContext();

        }

    }
}
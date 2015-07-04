using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeptEmpMgmt.Models;
using DeptEmpMgmt.CustomFilters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace DeptEmpMgmt.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        private ApplicationDbContext context = new ApplicationDbContext();

        [AuthLog(Roles = "Admin, Employee")]
        // GET: Employees
        // [Authorize(Roles = "Admin, Employee")]
        public ActionResult Index()
        {
            var users = context.Users.Include(e => e.Department.DepartmentName);
            return View(users.ToList());
        }

        // GET: Employees/Details/5
        [AuthLog(Roles = "Admin, Employee")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Employee employee = context.Employees.Find(id);
            ApplicationUser user = context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Employees/Create
        [AuthLog(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.Roles = new SelectList(context.Roles.ToList(), "Name", "Name");
            ViewBag.DepartmentId = new SelectList(context.Departments, "DepartmentId", "DepartmentName");

            return View();
        }
        //Create Employee


        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            var user = context.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));
            UserManager.AddToRole(user.Id, RoleName);
            ViewBag.ResultMessage = "Role created successfully !";
            // prepopulate roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View();
        }


        public ActionResult SendPassword()
        {
            return View();
        }

        // GET: Employees/Edit/5
        [AuthLog(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = context.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,EmployeeName,DepartmentId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                context.Entry(employee).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            return View(employee);
        }
        public ActionResult Delete(string id)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(context.Users.Find(id));
        }


        /// POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser user = context.Users.Find(id);

            context.Users.Remove(user);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GenerateRandomPassword(ApplicationUser user)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            string PasswordLength = "12";
            string NewPassword = "";

            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "~,!,@,#,$,%,^,&,*,+,?";

            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);

            string IDString = "";
            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;
            }
            context.SaveChanges();

            user.RandomPassword = NewPassword;
            context.Users.Add(user);

            context.SaveChanges();
            return NewPassword;
        }
    }
}

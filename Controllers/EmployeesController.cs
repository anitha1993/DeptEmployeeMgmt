using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeptEmpMgmt.Models;
using DeptEmpMgmt.CustomFilters;
using System.Configuration;
//using DeptEmpMgmt.Logic;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

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

            //var employees = context.Employees.Include(e => e.Department);
            //return View(employees.ToList());
            var users = context.Users.Include(e => e.Departments.DepartmentName);
            return View(context.Users.ToList());
        }

        // GET: Employees/Details/5
        [AuthLog(Roles = "Admin")]
        public ActionResult Details(int? id)
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
            return View(employee);
        }

        // GET: Employees/Create
        [AuthLog(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.Roles = new SelectList(context.Roles.ToList(), "Name", "Name");
            // string selectedRoleName = Roles.SelectedValue;
            ViewBag.DepartmentId = new SelectList(context.Departments, "DepartmentId", "DepartmentName");

            return View();
        }
        //Create Employee
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async System.Threading.Tasks.Task<ActionResult> Create(ApplicationUser user, string RoleName)
        ////{
        ////    using (var context = new ApplicationDbContext())
        ////    {
        ////        if (ModelState.IsValid)
        ////        {
        ////            //string Email= User.Email;
        ////            //string Email = employee.Email;
        ////            //string password = GenerateRandomPassword(employee);
        ////            string password = GenerateRandomPassword(user);
        ////            string sendEmail = ConfigurationManager.AppSettings["SendEmail"];
        ////            SendMail(Email, sendEmail, password);
        ////            var user = new ApplicationUser { UserName = employee.UserName, Email = employee.Email };
        ////            var result = await UserManager.CreateAsync(user, employee.RandomPassword);
        ////            var roleStore = new RoleStore<IdentityRole>(context);
        ////            var roleManager = new RoleManager<IdentityRole>(roleStore);
        ////            var userStore = new UserStore<ApplicationUser>(context);
        ////            var userManager = new UserManager<ApplicationUser>(userStore);
        ////            userManager.AddToRole(user.Id, RoleName);
        ////            var employeeid = employee.EmployeeId;
        ////            if (result.Succeeded)
        ////            {
        ////                return RedirectToAction("Index", "Home");
        ////            }
        ////        }

        ////        // If we got this far, something failed, redisplay form
        ////        return View(employee);
        ////    }

        ////}


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
        public ActionResult Edit(int? id)
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







        //vvvvvvvvvvvv
        ////[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        ////public async Task<ActionResult> DeleteConfirmed(string id)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        if (id == null)
        ////        {
        ////            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        ////        }

        ////        var user = await context.Users.FindAsync(id);
        ////        var logins = user.Logins;
        ////        foreach (var login in logins)
        ////        {
        ////            context.UserLogins.Remove(login);
        ////        }
        ////        var rolesForUser = await IdentityManager.Roles.GetRolesForUserAsync(id, CancellationToken.None);
        ////        if (rolesForUser.Count() > 0)
        ////        {

        ////            foreach (var item in rolesForUser)
        ////            {
        ////                var result = await IdentityManager.Roles.RemoveUserFromRoleAsync(user.Id, item.Id, CancellationToken.None);
        ////            }
        ////        }
        ////        context.Users.Remove(user);
        ////        await context.SaveChangesAsync();
        ////        return RedirectToAction("Index");
        ////    }
        ////    else
        ////    {
        ////        return View();
        ////    }
        ////}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        //{

        //    var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
        //    if (result.Succeeded)
        //    {
        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //        if (user != null)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //        }

        //    }

        //    return RedirectToAction("Index");
        //}





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
        //public string GenerateRandomPassword(Application  employee)
        //{
        //    // Employee employee = new Employee();
        //    ApplicationDbContext context = new ApplicationDbContext();

        //    string PasswordLength = "12";
        //    string NewPassword = "";
        //    //employee.RandomPassword = NewPassword;


        //    string allowedChars = "";
        //    allowedChars = "1,2,3,4,5,6,7,8,9,0";
        //    allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
        //    allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
        //    allowedChars += "~,!,@,#,$,%,^,&,*,+,?";



        //    char[] sep = { ',' };
        //    string[] arr = allowedChars.Split(sep);

        //    string IDString = "";
        //    string temp = "";


        //    Random rand = new Random();


        //    for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
        //    {
        //        temp = arr[rand.Next(0, arr.Length)];
        //        IDString += temp;
        //        NewPassword = IDString;
        //    }
        //    context.SaveChanges();


        //    employee.RandomPassword = NewPassword;
        //    context.Employees.Add(employee);
        //    var employeeid = employee.EmployeeId;

        //    context.SaveChanges();
        //    // return new { NewPassword, employeeid }; with t
        //    return NewPassword;


        //}
        public string GenerateRandomPassword(ApplicationUser user)
        {
            // Employee employee = new Employee();
            ApplicationDbContext context = new ApplicationDbContext();

            string PasswordLength = "12";
            string NewPassword = "";
            //employee.RandomPassword = NewPassword;


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

            // var employeeid = employee.EmployeeId;

            context.SaveChanges();
            // return new { NewPassword, employeeid }; with t
            return NewPassword;


        }

        ////private void SendEMail(string emailid, string subject, string body)
        ////{
        ////    SmtpClient client = new SmtpClient();
        ////    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        ////    client.EnableSsl = true;
        ////    client.Host = "smtp.gmail.com";
        ////    client.Port = 587;


        ////    NetworkCredential credentials = new NetworkCredential("mothisanithaaloysius@gmail.com", "amazinggrace");
        ////    client.UseDefaultCredentials = false;
        ////    client.Credentials = credentials;

        ////    MailMessage msg = new MailMessage();
        ////    msg.From = new MailAddress("mothisanithaaloysius@gmail.com");
        ////    msg.To.Add(new MailAddress(emailid));

        ////    msg.Subject = subject;
        ////    msg.IsBodyHtml = true;
        ////    msg.Body = body;

        ////    client.Send(msg);
        ////}
        public void SendMail(string Email, string emailBody, string password)
        {
            Employee employee = new Employee();
            SmtpClient smtpClient = new SmtpClient();

            //   var mailMessage = new MailMessage(((NetworkCredential)(smtpClient.Credentials)).UserName, to, subject, body);

            var mailMessage = new MailMessage(((NetworkCredential)(smtpClient.Credentials)).UserName, Email, "User confirmation", "Thank you for your registration! Your Password is <b>" + password + "</b> Please login and change the password.");

            // MailMessage mailMessage = new MailMessage();

            //////var to = mailMessage.To.Add(Email);
            //////var subject = mailMessage.Subject = "User confirmation";
            //////var body = mailMessage.IsBodyHtml = true;
            //////mailMessage.Body = string.Format("Thank you for your registration! Your Password is <b>" + password + "</b> Please login and change the password.");

            //  smtpClient.UseDefaultCredentials = true;
            smtpClient.Send(mailMessage);
        }

    }
}

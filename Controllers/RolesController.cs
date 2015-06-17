﻿using System;
using System.Linq;
using System.Web.Mvc;

using DeptEmpMgmt.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DeptEmpMgmt.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext context;

        public RoleController()
        {
            context = new ApplicationDbContext();
        }

         /// Get All Roles
       
        public ActionResult Index()
        {
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        
        /// Create  a New role
      
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

       
        /// Create a New Role
      
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
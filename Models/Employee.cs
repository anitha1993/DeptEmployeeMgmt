using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeptEmpMgmt.Models
{

    public class Employee
    {
        public int EmployeeId { get; set; }

        [DisplayName("User Name")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Email")]
       // [Required]
        public string Email { get; set; }

        public string RandomPassword { get; set; }
        
        public string Roles { get; set; }

        [DisplayName("Department Id")]
        public int DepartmentId { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Department Department { get; set; }
    }


}

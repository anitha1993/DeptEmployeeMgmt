using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeptEmpMgmt.Models
{

    public class Employee
    {
        public int EmployeeId { get; set; }

        [DisplayName("Employee Name")]
        [Required]
        public string EmployeeName { get; set; }

        [DisplayName("Email")]
        [Required]
        public string Email { get; set; }

        [DisplayName("Department Id")]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }


}

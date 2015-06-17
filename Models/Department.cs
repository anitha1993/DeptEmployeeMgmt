using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeptEmpMgmt.Models
{
   
        public class Department
        {
            public int DepartmentId { get; set; }

            [DisplayName("Department Name")]
            [Required]
            public string DepartmentName { get; set; }

            public virtual ICollection<Employee> Employees { get; set; }
        }
    
}

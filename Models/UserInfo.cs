using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeptEmpMgmt.Models
{
    public class UserInfo
    {
        public string FullName { get; set; }
        public string Email{ get; set; }
        public int UserInfoId{ get; set; }

        
      //  public virtual ApplicationUser ApplicationUser { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class User
    {
        public int UserId { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string MobileNumber { set; get; }
        public string NationalIdentityCard { set; get; }
        public string ManagerName { set; get; }
        public string DepartmentName { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
    }
}

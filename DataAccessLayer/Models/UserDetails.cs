using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class UserDetails
    {
        public int UserID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string MobileNumber { set; get; }
        public string NationalIdentityCard { set; get; }
        public string ManagerName { set; get; }

    }
}

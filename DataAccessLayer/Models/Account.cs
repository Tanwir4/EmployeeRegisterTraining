using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Account
    {
        public int UserAccountId { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
    }
}

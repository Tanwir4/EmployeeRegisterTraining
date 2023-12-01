using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer.Models
{
    public class ApplicationDetails
    {
        public int ApplicationId { set; get; }
        public DateTime ApplicationDate { set; get; }
        public string Status { set; get; }
        public bool ManagerApproval { set; get; }
        public string DeclineReason { set; get; }

    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class EnrolledEmployeeForExportDTO
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string TrainingTitle { get; set; }
        public string Email {  get; set; }
        public string ManagerFirstName {  get; set; }
        public string ManagerLastName { get; set; }

    }
}

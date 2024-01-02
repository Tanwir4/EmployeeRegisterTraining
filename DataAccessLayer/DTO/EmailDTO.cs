using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class EmailDTO
    {
        public string ApplicantName { get; set; }
        public string Manager {  get; set; }
        public string ApplicationStatus {  get; set; }
        public string TrainingTitle { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class ProcessApplicationDTO
    {
        public string ApplicantName {  get; set; }
        public string TrainingTitle {  get; set; }
        public string Status {  get; set; }
        public bool ManagerApproval {  get; set; }
        public string DeclineReason { get; set; }
    }
}

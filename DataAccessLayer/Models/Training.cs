using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Entities
{
    public class Training
    {
        public int TrainingId { set; get; }
        public string Title { set; get; }
        public DateTime StartDate { set; get; }
        public int Threshold { set; get; }
        public string PreRequisite { set; get; }
        public int Priority { set; get; }
    }
}

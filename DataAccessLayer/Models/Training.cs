using System;
using System.Collections.Generic;

namespace EmployeeTrainingRegistrationServices.Entities
{
    public class Training
    {
        public int TrainingID { set; get; }
        public string Title { set; get; }
        public DateTime StartDate { set; get; }
        public int Threshold { set; get; }
        public string Description { set; get; }
        public int DepartmentPriority { set; get; }
        public DateTime Deadline { set; get; }
        public List<string> PreRequisite {  set; get; }
    }
}

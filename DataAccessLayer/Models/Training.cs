using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTrainingRegistrationServices.Entities
{
    public class Training
    {
        public int TrainingID { set; get; }
        [Required]
        public string Title { set; get; }
        [Required]
        public DateTime StartDate { set; get; }
        [Required]
        public int Threshold { set; get; }
        [Required]
        public string Description { set; get; }
        [Required]
        public int DepartmentPriority { set; get; }
        [Required]
        public DateTime Deadline { set; get; }
        [Required]
        public List<string> PreRequisite {  set; get; }
        [Required]
        public string DepartmentName { set; get; }
    }
}

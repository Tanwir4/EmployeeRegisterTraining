using System.ComponentModel.DataAnnotations;

namespace EmployeeTrainingRegistrationServices.Entities
{
    public class Department
    {
        public int DepartmentId { set; get; }
        [Required]
        public string DepartmentName { set; get; }
    }
}

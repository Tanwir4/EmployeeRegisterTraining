using System.ComponentModel.DataAnnotations;

namespace EmployeeTrainingRegistrationServices.Entities
{
    public class Role
    {
        public int RoleId { set; get; }
        [Required]
        public string RoleName { set; get; }
    }
}

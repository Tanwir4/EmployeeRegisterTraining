using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class User
    {
        public int UserId { set; get; }
        [Required]
        public string FirstName { set; get; }
        [Required]
        public string LastName { set; get; }
        [Required]
        public string MobileNumber { set; get; }
        [Required]
        public string NationalIdentityCard { set; get; }
        [Required]
        public string ManagerName { set; get; }
        [Required]
        public string DepartmentName { set; get; }
        [Required]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }
    }
}

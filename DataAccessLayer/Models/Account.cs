using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class Account
    {
        public int UserAccountId { set; get; }
        [Required]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }
    }
}

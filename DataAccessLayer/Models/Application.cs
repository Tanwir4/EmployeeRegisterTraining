using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DataAccessLayer.Models
{
    public class Application
    {
        public int ApplicationId { set; get; }
        [Required]
        public DateTime ApplicationDate { set; get; }
        [Required]
        public string Status { set; get; }
        public bool ManagerApproval { set; get; }
        public string DeclineReason { set; get; }
        [Required]
        public int TrainingId { set; get; }
        [Required]
        public int UserId {  set; get; }
        [Required]
        public string FirstName {  set; get; }
        [Required]
        public string LastName { set; get; }
        public string Email { set; get; }

    }
}

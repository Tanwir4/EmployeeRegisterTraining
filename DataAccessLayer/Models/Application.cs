﻿using System;
using System.Globalization;

namespace DataAccessLayer.Models
{
    public class Application
    {
        public int ApplicationId { set; get; }
        public DateTime ApplicationDate { set; get; }
        public string Status { set; get; }
        public bool ManagerApproval { set; get; }
        public string DeclineReason { set; get; }
        public int TrainingId { set; get; }
        public int UserId {  set; get; }
        public string FirstName {  set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }

    }
}

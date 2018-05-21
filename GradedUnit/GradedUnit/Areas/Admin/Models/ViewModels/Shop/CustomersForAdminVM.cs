using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnit.Areas.Admin.Models.ViewModels.Shop
{
    public class CustomersForAdminVM
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }


    }
}
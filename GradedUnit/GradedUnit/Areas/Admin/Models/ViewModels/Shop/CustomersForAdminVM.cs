//Mark Riley
//30/05/18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnit.Areas.Admin.Models.ViewModels.Shop
{
    /// <summary>
    /// View model class that is used to list all of the users in the system
    /// </summary>
    public class CustomersForAdminVM
    {
        /// <summary>
        /// Variable which will hold the UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Variable which holds the username of the user
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Variable which holds the First Name of the user
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Variable that holds the Last Name of the User
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Variable which will hold the email address of a user
        /// </summary>
        public string EmailAddress { get; set; }


    }
}
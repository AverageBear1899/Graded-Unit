//Mark Riley
//30/05/18

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.ViewModels.Account
{
    /// <summary>
    /// View model with attributes for the Login page
    /// </summary>
    public class LoginUserVM
    {
        /// <summary>
        /// Variable which holds the username
        /// </summary>
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// Variable which holds the password
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// bool that can be checked to remember the users details so they dont need to login next time they visit the page
        /// </summary>
        public bool RememberMe { get; set; }


    }
}
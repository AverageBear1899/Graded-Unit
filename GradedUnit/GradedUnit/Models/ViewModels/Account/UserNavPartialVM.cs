//Mark Riley
//30/05/18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.ViewModels.Account
{
    /// <summary>
    /// View model with attributes for the Users profile link
    /// </summary>
    public class UserNavPartialVM
    {
        /// <summary>
        /// variable which will hold the users first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// variable which will hold the users second name
        /// </summary>
        public string LastName { get; set; }
    }
}

//Mark Riley
//30/05/18
using GradedUnit.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.ViewModels.Account
{
    /// <summary>
    /// View model with attributes and constructors to build the user's profile page
    /// </summary>
    public class UserProfileVM
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public UserProfileVM()
        {

        }
        /// <summary>
        /// Constructor built with values from the table
        /// </summary>
        /// <param name="row"></param>
        public UserProfileVM(UserDTO row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            EmailAddress = row.EmailAddress;
            //Username = row.Username;
            Password = row.Password;
            Address = row.Address;
        }
        /// <summary>
        /// Variable which holds the unique id of a user
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Variable which holds the first name
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Variable which holds the last name
        /// </summary>
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// Variable which holds the email address
        /// </summary>
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailAddress { get; set; }
        //[Required]
        //public string Username { get; set; }
        /// <summary>
        /// Variable which holds the password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// variable which holds the password again to check they match
        /// </summary>
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// Variable which holds the address
        /// </summary>
        public string Address { get; set; }
    }
}
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
    /// View model with attributes and constructors for the user information 
    /// </summary>
    public class UserVM
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public UserVM()
        {

        }
        /// <summary>
        /// constructor populated with values from the table
        /// </summary>
        /// <param name="row"></param>
        public UserVM(UserDTO row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            EmailAddress = row.EmailAddress;
            Username = row.Username;
            Password = row.Password;
            Address = row.Address;
        }
        /// <summary>
        /// Variable which will hold the unique ID of a user
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Variable which will hold the first name of a user
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Variable which will hold the last name of a user
        /// </summary>
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// Variable which will hold the email address of a user
        /// </summary>
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailAddress { get; set; }
        /// <summary>
        /// Variable which will hold the username of a user
        /// </summary>
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// Variable which will hold the password of a user
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Variable which will hold the password again allowing us to check if they're the same
        /// </summary>
        [Required]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// Variable which holds the address of a user
        /// </summary>
        [Required]
        public string Address { get; set; }
    }
}
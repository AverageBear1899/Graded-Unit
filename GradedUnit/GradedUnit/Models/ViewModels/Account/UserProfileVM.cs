using GradedUnit.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.ViewModels.Account
{
    public class UserProfileVM
    {
        public UserProfileVM()
        {

        }
        public UserProfileVM(UserDTO row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            EmailAddress = row.EmailAddress;
            Username = row.Username;
            Password = row.Password;
            Address = row.Address;
        }
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
    }
}
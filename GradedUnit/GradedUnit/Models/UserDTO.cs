using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{ 
    /// <summary>
    /// This class will hold the information of a user
    /// </summary>
    [Table("tblUsers")]
    public class UserDTO
    {
        /// <summary>
        /// Unique identifier of the user
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// First name of the user
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last name of the user
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Holds the email address of the user
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Holds the username of the user
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Holds the password of the user
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Holds the address of the user
        /// </summary>
        public string Address { get; set; }


    }
}
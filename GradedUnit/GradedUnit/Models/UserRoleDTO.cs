using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{
    /// <summary>
    /// This class will hold the information of the user and the role that is assosiated with them
    /// </summary>
    [Table("tblUserRoles")]
    public class UserRoleDTO
    {
        /// <summary>
        /// Unique identifier of the User
        /// </summary>
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        /// <summary>
        /// Identifier of the role ID they've been given
        /// </summary>
        [Key, Column(Order = 1)]
        public int RoleId { get; set; }

        /// <summary>
        /// Foreign key that is the primary key from the User table
        /// </summary>
        [ForeignKey("UserId")]
        public virtual UserDTO User { get; set; }
        /// <summary>
        /// Foreign key that is the primary key from the Role table
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual RoleDTO Role { get; set; }

    }
}
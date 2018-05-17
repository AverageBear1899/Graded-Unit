using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{
    /// <summary>
    /// This class will hold information of the roles available
    /// </summary>
    [Table("tblRoles")]
    public class RoleDTO
    {
        /// <summary>
        /// Unique identifier of the role
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Name of the role
        /// </summary>
        public string Name { get; set; }
    }
}
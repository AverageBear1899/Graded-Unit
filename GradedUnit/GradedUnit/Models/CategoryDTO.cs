using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{
    /// <summary>
    /// This is the catagories class which is used to provide a product with a category and allow a user to select the category they wish to view
    /// </summary>
    [Table("tblCatagories")]
    public class CategoryDTO
    {
        /// <summary>
        /// Unique identifier for the category
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Name of the category given by an admin
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the category auto-generated when category is created
        /// </summary>
        public string Slug { get; set; }
        /// <summary>
        /// Identifier for where the category will appear in the list depending on its associated number
        /// </summary>
        public int Sorting { get; set; }
    }
}
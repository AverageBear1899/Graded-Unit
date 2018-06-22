//Mark Riley
//30/05/18

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{
    /// <summary>
    /// Class which will hold information of the pages
    /// </summary>
    [Table("tblPages")]
    public class PageDTO
    {
        /// <summary>
        /// Unique identifier for the the page
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The title of the page
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// A small auto-generated descriptor of the page
        /// </summary>
        public string Slug { get; set; }
        /// <summary>
        /// The actual content of the page which is input by the admin
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Allows the admin to change the order of how the pages appear by providing them with a sorting number
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// Boolean to identify whether or not a page will have a sidebar which can be filled with information
        /// </summary>
        public bool HasSidebar { get; set; }

    }
}
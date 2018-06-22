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
    /// This class holds the id and body of a sidebar
    /// </summary>
    [Table("tblSidebar")]
    public class SidebarDTO

    {
        /// <summary>
        /// Unique identifier of the sidebar
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Stores the inputted information that the sidebar will display
        /// </summary>
        public string Body { get; set; }
    }
}
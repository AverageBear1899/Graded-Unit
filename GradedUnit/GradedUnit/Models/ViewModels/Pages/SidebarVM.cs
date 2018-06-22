//Mark Riley
//30/05/18

using GradedUnit.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Models.ViewModels.Pages
{
    /// <summary>
    /// View model for the sidebar with attributes to build the sidebars on pages that have it
    /// </summary>
    public class SidebarVM
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public SidebarVM()
        {

        }
        /// <summary>
        /// Constructor loaded with values from table
        /// </summary>
        /// <param name="row"></param>
        public SidebarVM(SidebarDTO row)
        {
            Id = row.Id;
            Body = row.Body;
        }
        /// <summary>
        /// Variable which holds a unique id for the sidebar
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Variable which will hold the body of the sidebar
        /// </summary>
        [AllowHtml]
        public string Body { get; set; }
    }
}
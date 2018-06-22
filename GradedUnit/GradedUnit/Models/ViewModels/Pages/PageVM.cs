//Mark Riley
//30/05/18

using GradedUnit.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Models.ViewModels.Pages
{
    /// <summary>
    /// View model with attributes and constructors to build the pages
    /// </summary>
    public class PageVM
    {
        /// <summary>
        /// Empty Contstructor
        /// </summary>
        public PageVM()
        {

        }
        /// <summary>
        /// Contructor made with values from the table
        /// </summary>
        /// <param name="row"></param>
        public PageVM(PageDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
        }
        /// <summary>
        /// Variable which holds the unique id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Variable which will hold the title of a page
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength =3)]
        public string Title { get; set; }
        /// <summary>
        /// Variable which will hold the slug of a page which is just a small unique descriptor
        /// </summary>
        public string Slug { get; set; }
        /// <summary>
        /// Variable which holds the body of a page
        /// </summary>
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [AllowHtml]
        public string Body { get; set; }
        /// <summary>
        /// Variable which will hold the sorting of a page to allow reorganisation of the pages
        /// </summary>
        public int Sorting { get; set; }
        /// <summary>
        /// bool which will determine whether or not a page will have a sidebar
        /// </summary>
        public bool HasSidebar { get; set; }
    }
}
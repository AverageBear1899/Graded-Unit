//Mark Riley
//30/05/18

using GradedUnit.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.ViewModels.Shop
{
    /// <summary>
    /// View model with attributes and constructors for the categories that have been created
    /// </summary>
    public class CategoryVM
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public CategoryVM()
        {

        }
        /// <summary>
        /// Constructor with values from the table
        /// </summary>
        /// <param name="row"></param>
        public CategoryVM(CategoryDTO row)

        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Sorting = row.Sorting;
        }
        /// <summary>
        /// variable which holds the unique id of a category
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Variable which will hold the name of a category
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Variable which will hold the slug of a category, which is used as a small descriptor
        /// </summary>
        public string Slug { get; set; }
        /// <summary>
        /// variable which holds the sorting of the category so i can change the way it appears in the menu
        /// </summary>
        public int Sorting { get; set; }
    }
}
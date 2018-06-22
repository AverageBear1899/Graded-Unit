//Mark Riley
//30/05/18

using GradedUnit.Models;
using GradedUnit.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Models.ViewModels.Shop
{
    /// <summary>
    /// View model with attributes and constructors for the products pages
    /// </summary>
    public class ProductVM
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public ProductVM()
        {

        }
        /// <summary>
        /// constructor loaded with values from the table
        /// </summary>
        /// <param name="row"></param>
        public ProductVM(ProductDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Description = row.Description;
            Price = row.Price;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
            ImageName = row.ImageName;
            Quantity = row.Quantity;
        }
        /// <summary>
        /// variable which holds the unique id of a product
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Variable which holds the name of a product
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// variable which holds the slug of a product which is a small descriptor
        /// </summary>
        public string Slug { get; set; }
        /// <summary>
        /// Variable which will hold the description of a product
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// Variable which will hold the price of a product
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Variable which will hold the category name of the product
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// Variable which holds the category id associated with the product
        /// </summary>
        [Required]
        public int CategoryId { get; set; }
        /// <summary>
        /// Variable which will hold the image path of the image for the product
        /// </summary>
        public string ImageName { get; set; }
        /// <summary>
        /// variable which will hold the quantity of a product
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Variable which is used to hold error messages
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// List of categories available
        /// </summary>
        public IEnumerable <SelectListItem> Categories { get; set; }
        /// <summary>
        /// This holds all of the image paths for the gallery of images
        /// </summary>
        public IEnumerable<string> GalleryImages { get; set; }
    }
}
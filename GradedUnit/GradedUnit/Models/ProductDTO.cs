using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{ 
    /// <summary>
    /// Class which will hold the data of the products in the store
    /// </summary>
    [Table("tblProducts")]
    public class ProductDTO
    {
    /// <summary>
    /// Unique identifier for the product
    /// </summary>
    [Key]
    public int Id { get; set; }
     /// <summary>
     /// Name of the product
     /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Brief auto-generated descriptor for the product
    /// </summary>
    public string Slug { get; set; }
    /// <summary>
    /// Holds the products description
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Holds the price of the product
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Holds the name of the category that the product is associated with
    /// </summary>
    public string CategoryName { get; set; }
    /// <summary>
    /// Identifier of the categories ID
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// Holds the image path of the uploaded image of the product
    /// </summary>
    public string ImageName { get; set; }
    /// <summary>
    /// Holds the quantity of the item
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Foreign key from the Category table
    /// </summary>
    [ForeignKey("CategoryId")]
    public virtual CategoryDTO Category { get; set; }


}
}
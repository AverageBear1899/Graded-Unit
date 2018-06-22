//Mark Riley
//30/05/18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.ViewModels.Cart
{
    /// <summary>
    /// View model with attributes for the cart
    /// </summary>
    public class CartVM
    {
        /// <summary>
        /// Variable which holds the unique id of a product
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Variable which holds the products name
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Variable which will hold the amount ordered
        /// </summary>
        public int QuantityOrdered { get; set; }
        /// <summary>
        /// Variable which holds the price of a product
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Variable which holds the total of the product
        /// </summary>
        public decimal Total { get { return QuantityOrdered * Price; } }
        /// <summary>
        /// Variable which holds the image path of the image of the product
        /// </summary>
        public string Image { get; set; }
    }
}
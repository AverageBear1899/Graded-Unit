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
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantityOrdered { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return QuantityOrdered * Price; } }
        public string Image { get; set; }
    }
}
//Mark Riley
//30/05/18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.ViewModels.Account
{
    /// <summary>
    /// View model for the Orders page for customers
    /// </summary>
    public class OrdersForUserVM
    {
        /// <summary>
        /// Variable which will hold the unique order number
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// Variable which will hold the total
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Variable which will hold the products and quantity ordered
        /// </summary>
        public Dictionary<string, int> ProductsAndQty { get; set; }
        /// <summary>
        /// Variable which holds the date the order was made
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
//Mark Riley
//30/05/18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnit.Areas.Admin.Models.ViewModels.Shop
{
    /// <summary>
    /// View model for the OrdersForAdmin page
    /// </summary>
    public class OrdersForAdminVM
    {
        /// <summary>
        /// Variable which will hold the order number
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// Variable which will hold the username assosiated with the order
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Variable which will hold the total of the order
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Dictionary that will hold the products ordered and the quantity ordered
        /// </summary>
        public Dictionary<string, int> ProductsAndQty { get; set; }
        /// <summary>
        /// Variable which holds the time the order was made
        /// </summary>
        public DateTime CreatedAt { get; set; }


    }
}
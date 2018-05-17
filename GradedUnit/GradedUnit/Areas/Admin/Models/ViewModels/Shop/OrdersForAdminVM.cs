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
        
        public int OrderNumber { get; set; }
        public string Username { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQty { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
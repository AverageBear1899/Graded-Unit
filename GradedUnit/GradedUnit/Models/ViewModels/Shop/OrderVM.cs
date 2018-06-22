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
    /// View model with attributes and constructors for the viewing of orders
    /// </summary>
    public class OrderVM
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public OrderVM()
        {

        }
        /// <summary>
        /// Constructor built with values from the table
        /// </summary>
        /// <param name="row"></param>
        public OrderVM(OrderDTO row)
        {
            OrderId = row.OrderId;
            UserId = row.UserId;
            CreatedAt = row.CreatedAt;
        }
        /// <summary>
        /// variable which holds the unique Id of the order
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Variable which holds the unique id of the user
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Variable which holds the datetime the order is made
        /// </summary>
        public DateTime CreatedAt { get; set; }

    }
}
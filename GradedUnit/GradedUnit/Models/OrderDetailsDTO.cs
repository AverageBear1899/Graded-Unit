//Mark Riley
//30/05/18

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{
    /// <summary>
    /// This is the OrderDetails class which helps to form a bridge between the many to many relationship between products and orders.
    /// </summary>
    [Table("tblOrderDetails")]
    public class OrderDetailsDTO
    {
        /// <summary>
        /// Unique identifier for the order details
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Unique identifier for the order
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Unique identfier for the productID
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Quantity of the item that has been ordered
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Foreign Key that is a the primary key of the Orders table
        /// </summary>
        [ForeignKey("OrderId")]
        public virtual OrderDTO Orders { get; set; }
        /// <summary>
        /// Foreign Key that is the primary key of the Products table
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ProductDTO Products { get; set; }
    }
}
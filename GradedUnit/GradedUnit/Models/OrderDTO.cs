using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{ 
    /// <summary>
    /// This is the orders class which will hold information of a customers order, when it was made and who by
    /// </summary>
    [Table("tblOrders")]
    public class OrderDTO
    {
        /// <summary>
        /// Unique identifier for the order
        /// </summary>
        [Key]
        public int OrderId { get; set; }
        /// <summary>
        /// Holds the information of when the order was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Holds the userId to allow identification of who placed the order
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Foriegn key that is the primary key from the Users table
        /// </summary>
        [ForeignKey("UserId")]
        public virtual UserDTO Users { get; set; }
    }
}
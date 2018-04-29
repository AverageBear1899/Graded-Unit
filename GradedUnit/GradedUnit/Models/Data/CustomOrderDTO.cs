using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.Data
{
    [Table("tblCustomOrder")]
    public class CustomOrderDTO
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string DeliveryAddress { get; set; }
        public string ImageLink { get; set; }
        public string Garment { get; set; }

    }
}
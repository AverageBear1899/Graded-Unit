using GradedUnit.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnit.Models.ViewModels.Pages
{
    public class CustomOrderVM
    {
        public CustomOrderVM()
        {

        }

        public CustomOrderVM(CustomOrderDTO row)
        {
            Id = row.Id;
            Email = row.Email;
            DeliveryAddress = row.DeliveryAddress;
            ImageLink = row.ImageLink;
            Garment = row.Garment;

        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string DeliveryAddress { get; set; }
        public string ImageLink { get; set; }
        public string Garment { get; set; }
    }
}
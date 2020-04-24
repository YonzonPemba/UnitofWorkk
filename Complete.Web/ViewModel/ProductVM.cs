using Complete.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Complete.Web.ViewModel
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string Descriptions { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int SupplierId { get; set; }
        public DateTime DateCreated { get; set; }


        public int QuantityId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Rate { get; set; }
    }
}
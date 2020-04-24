using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Complete.Web.ViewModel
{
    public class BillingProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string Descriptions { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }

        public string Rate { get; set; }

        public int Quantity { get; set; }

        public string Discount { get; set; }
    }
}
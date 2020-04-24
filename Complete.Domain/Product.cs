//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Complete.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.BillingProducts = new HashSet<BillingProduct>();
            this.ProductStocks = new HashSet<ProductStock>();
            this.Quantities = new HashSet<Quantity>();
        }
    
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string Descriptions { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateModfied { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingProduct> BillingProducts { get; set; }
        
        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quantity> Quantities { get; set; }
    }
}

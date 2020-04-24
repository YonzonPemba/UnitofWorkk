using Complete.Domain;
using Complete.Repository.Repositories;
using Complete.Service.CustomServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Service.EntityServices
{
   public  class ProductVMService
    {
        private ProductService productService = new ProductService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Product>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        //private QuantityService quantityService = new QuantityService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Quantity>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        //private CategoryService categoryService = new CategoryService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Category>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        //private SupplierService supplierService = new SupplierService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<Supplier>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        private BillingProductService billingProductService = new BillingProductService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<BillingProduct>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));

        public bool CheckDuplicate(string ProductName,int CategoryId,int SupplierId)
        {
            var products = productService.Load(x => x.DateDeleted == null).ToList();
            if (products.Any(x => x.ProductName == ProductName && x.CategoryId == CategoryId && x.SupplierId == SupplierId))
            {
                return true;
            }
            return false;
        }

        public bool UpdateValidate(string ProductName,int CategoryId,int SupplierId,int ProductId)
        {
            var products = productService.Load(x => x.DateDeleted == null && x.ProductId !=ProductId).ToList();
            if (products.Any(x => x.ProductName == ProductName && x.CategoryId == CategoryId && x.SupplierId == SupplierId))
            {
                return false;
            }
            return true;
        }

        public bool ValidateDelete(Product product)
        {
            List<BillingProduct> billingProducts = billingProductService.Load(x => x.ProductId == product.ProductId).ToList();
            if (billingProducts.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}

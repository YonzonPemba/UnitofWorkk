using Complete.Domain;
using Complete.Repository.Contracts;
using Complete.Repository.Repositories;
using Complete.Service.CustomServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Service.EntityServices
{
   public class ProductService:GenericService<Product>
    {
        private BillingProductService billingProductService = new BillingProductService(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities")), new EFRepository<BillingProduct>(new EFUnitOfWork(new DbContextFactory("SmartInventoryEntities", "SmartInventoryEntities"))));
        public ProductService(IUnitOfWork unitOfWork,IRepository<Product> repository):base(unitOfWork,repository)
        { 
            
        }
        public bool CheckDuplicate(Product product)
        {
            var products = repository.Load(x => x.DateDeleted == null).ToList();
            if (products.Any(x => x.ProductName == product.ProductName&&x.CategoryId==product.CategoryId&&x.SupplierId==product.SupplierId))
            {
                return true;
            }
            return false;
        }

        public bool UpdateValidate(Product product)
        {
            var products = repository.Load(x => x.DateDeleted == null && x.ProductId != product.ProductId).ToList();
            if (products.Any(x => x.ProductName == product.ProductName && x.CategoryId == product.CategoryId && x.SupplierId == product.SupplierId))
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

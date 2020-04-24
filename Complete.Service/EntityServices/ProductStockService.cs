using Complete.Domain;
using Complete.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Service.EntityServices
{
    public class ProductStockService:GenericService<ProductStock>
    {
        public ProductStockService(IUnitOfWork unitOfWork,IRepository<ProductStock> repository) : base(unitOfWork, repository)
        { 
            
        }

        public bool CheckDuplicate(ProductStock productStock)
        {
            var productStocks = repository.Load(x => x.DateDeleted == null&&x.ProductId==productStock.ProductId).ToList();
            if (productStocks.Count > 0) {
                return true; ;
            }
            return false;
        }

        public bool UpdateValidate(ProductStock productStock)
        {
            if (productStock.MinimumStockQuantity < 0)
            {
                return false;
            }
            return true;
        }

        //public bool ValidateDelete(ProductStock productStock)
        //{
        //    List<BillingProductStock> billingProductStocks = billingProductStockService.Load(x => x.ProductStockId == product.ProductStockId).ToList();
        //    if (billingProductStocks.Count > 0)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}

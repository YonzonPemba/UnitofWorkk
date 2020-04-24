using Complete.Domain;
using Complete.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Service.EntityServices
{
    public class BillingProductService:GenericService<BillingProduct>
    {
        public BillingProductService(IUnitOfWork unitOfWork, IRepository<BillingProduct> repository) : base(unitOfWork, repository)
        {
            
        }
    }
}

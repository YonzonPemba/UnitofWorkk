using Complete.Domain;
using Complete.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Service.EntityServices
{
   public class QuantityService:GenericService<Quantity>
    {
        public QuantityService(IUnitOfWork unitOfWork,IRepository<Quantity> repository) : base(unitOfWork, repository) { 
            
        }
    }
}

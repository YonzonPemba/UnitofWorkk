using Complete.Domain;
using Complete.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Service.EntityServices
{
    public class BillingService : GenericService<Billing>
    {
        public BillingService(IUnitOfWork unitOfWork, IRepository<Billing> repository) : base(unitOfWork, repository)
        {

        }
    }
}

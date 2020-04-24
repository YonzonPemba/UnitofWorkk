using Complete.Domain;
using Complete.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Service.EntityServices
{
   public class SupplierService:GenericService<Supplier>
    {
        public SupplierService(IUnitOfWork unitOfWork, IRepository<Supplier> repository) : base(unitOfWork, repository) { 
        
        }

        public bool CheckDuplicate(Supplier supplier)
        {
            var suppliers = repository.Load(x => x.DateDeleted == null).ToList();
            if (suppliers.Any(x => x.SupplierName == supplier.SupplierName))
            {
                return true;
            }
            return false;
        }
        public bool UpdateValidate(Supplier supplier)
        {
            var suppliers = repository.Load(x => x.DateDeleted == null && x.SupplierId != supplier.SupplierId).ToList();
            if (suppliers.Any(x => x.SupplierName == supplier.SupplierName))
            {
                return false;
            }
            return true;
        }

        public bool ValidateDelete(Supplier supplier)
        {
            Supplier supp = repository.LoadByID(supplier.SupplierId);
            if (supp.Products.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}

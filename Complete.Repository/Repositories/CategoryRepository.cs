using Complete.Domain;
using Complete.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Repository.Repositories
{
   public class CategoryRepository:EFRepository<Category>,ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public List<Category> TopCategories()
        {
            return null;
        }



    }
}

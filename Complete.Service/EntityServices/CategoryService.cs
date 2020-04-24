using Complete.Domain;
using Complete.Repository.Contracts;
using Complete.Repository.Repositories;
using Complete.Service.Contracts;
using Complete.Service.EntityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Complete.Service.CustomServices
{
    public class CategoryService:GenericService<Category>,ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork,IRepository<Category> repository):base(unitOfWork,repository)
        {
        }

        public bool CheckDuplicate(Category category)
        {
            var categories=repository.Load(x=>x.DateDeleted==null).ToList();
            if (categories.Any(x => x.CategoryName == category.CategoryName)) {
                return true;
            }
            return false;
        }
        public List<Category> TopCategories()
        {
            return null;
        }

        public bool UpdateValidate(Category category)
        {
            var categories = repository.Load(x => x.DateDeleted == null && x.CategoryId != category.CategoryId).ToList();
            if (categories.Any(x => x.CategoryName == category.CategoryName)) {
                return false;
            }
            return true;
        }

        public bool ValidateDelete(Category category)
        {
            Category cat = repository.LoadByID(category.CategoryId);
            if (cat.Products.Count > 0) {
                return false;
            }
            return true;
        }
    }
}

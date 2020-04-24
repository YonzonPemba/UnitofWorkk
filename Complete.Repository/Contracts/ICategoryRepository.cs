using Complete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Repository.Contracts
{
    interface ICategoryRepository:IRepository<Category>
    {
        List<Category> TopCategories();
    }
}

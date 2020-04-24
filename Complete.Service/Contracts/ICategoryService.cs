using Complete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Service.Contracts
{
    public interface ICategoryService:IGenericService<Category>
    {
        List<Category> TopCategories(); 
    }
}

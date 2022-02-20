using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> Find(int categoryId);
        Task<Category> FindByName(string name);
        Task<bool> AddCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(Category category);
        IList<Category> GetAllCategories();
    }
}

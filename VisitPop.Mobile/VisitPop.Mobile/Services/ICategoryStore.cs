using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;

namespace VisitPop.Mobile.Services
{
    public interface ICategoryStore
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategory(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(Category category);
    }
}

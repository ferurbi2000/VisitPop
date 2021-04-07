using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Category;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PagedList<Category>> GetCategoriesAsync(CategoryParametersDto categoryParameters);

        Task<Category> GetCategoryAsync(int categoryId);

        Category GetCategory(int categoryId);

        Task AddCategory(Category category);

        void DeleteCategory(Category category);
        void UpdateCategory(Category category);

        bool Save();

        Task<bool> SaveAsync();
    }
}

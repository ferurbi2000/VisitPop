using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Category;
using VisitPop.Application.Interfaces;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public CategoryRepository(VisitPopDbContext context, SieveProcessor sieveProcessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Category>> GetCategoriesAsync(CategoryParametersDto categoryParameters)
        {
            if (categoryParameters == null)
                throw new ArgumentNullException(nameof(categoryParameters));

            var collection = _context.Categories
                as IQueryable<Category>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = categoryParameters.SortOrder ?? "CategoryId",
                Filters = categoryParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Category>.CreateAsync(collection,
                categoryParameters.PageNumber,
                categoryParameters.PageSize);

        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            // include marker -- requires return _context.Categorys as it's own line with no extra text -- do not delete this comment
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public Category GetCategory(int categoryId)
        {
            // include marker -- requires return _context.Categorys as it's own line with no extra text -- do not delete this comment
            return _context.Categories
                .FirstOrDefault(c => c.Id == categoryId);
        }


        public async Task AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(Category));

            await _context.Categories.AddAsync(category);
        }

        public void DeleteCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(Category));

            _context.Categories.Remove(category);
        }

        public void UpdateCategory(Category category)
        {
            // no implementation for now
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


    }
}

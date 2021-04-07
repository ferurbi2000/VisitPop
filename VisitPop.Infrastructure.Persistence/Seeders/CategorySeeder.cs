using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class CategorySeeder
    {
        public static void SeedSampleCategoryData(VisitPopDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.Add(new AutoFaker<Category>());
                context.Categories.Add(new AutoFaker<Category>());
                context.Categories.Add(new AutoFaker<Category>());
                context.SaveChanges();
            }
        }
    }
}

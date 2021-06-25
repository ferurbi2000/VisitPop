using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class VisitTypeSeeder
    {
        public static void VisitTypeSampleData(VisitPopDbContext context)
        {
            if (!context.VisitTypes.Any())
            {
                context.VisitTypes.Add(new AutoFaker<VisitType>());
                context.VisitTypes.Add(new AutoFaker<VisitType>());
                context.VisitTypes.Add(new AutoFaker<VisitType>());
                context.VisitTypes.Add(new AutoFaker<VisitType>());

                context.SaveChanges();
            }
        }
    }
}

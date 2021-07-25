using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class VisitSeeder
    {
        public static void SeedSampleVisitData(VisitPopDbContext context)
        {
            if (!context.Visits.Any())
            {
                context.Visits.Add(new AutoFaker<Visit>());
                context.Visits.Add(new AutoFaker<Visit>());
                context.Visits.Add(new AutoFaker<Visit>());
                context.Visits.Add(new AutoFaker<Visit>());
                context.Visits.Add(new AutoFaker<Visit>());
                context.Visits.Add(new AutoFaker<Visit>());
                context.Visits.Add(new AutoFaker<Visit>());
                context.Visits.Add(new AutoFaker<Visit>());
                context.Visits.Add(new AutoFaker<Visit>());

                context.SaveChanges();
            }
        }
    }
}

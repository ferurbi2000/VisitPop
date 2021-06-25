using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class OfficeSeeder
    {

        public static void SeedSampleOfficeData(VisitPopDbContext context)
        {
            if (!context.Offices.Any())
            {
                context.Offices.Add(new AutoFaker<Office>());
                context.Offices.Add(new AutoFaker<Office>());
                context.Offices.Add(new AutoFaker<Office>());
                context.Offices.Add(new AutoFaker<Office>());

                context.SaveChanges();
            }
        }
    }
}

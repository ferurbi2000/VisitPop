using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class VisitPersonSeeder
    {
        public static void VisitPersonSampleData(VisitPopDbContext context)
        {
            if (!context.VisitPersons.Any())
            {
                context.VisitPersons.Add(new AutoFaker<VisitPerson>());
                context.VisitPersons.Add(new AutoFaker<VisitPerson>());
                context.VisitPersons.Add(new AutoFaker<VisitPerson>());
                context.VisitPersons.Add(new AutoFaker<VisitPerson>());
                context.VisitPersons.Add(new AutoFaker<VisitPerson>());
                context.VisitPersons.Add(new AutoFaker<VisitPerson>());

                context.SaveChanges();
            }
        }
    }
}

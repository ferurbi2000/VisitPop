using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class PersonTypeSeeder
    {
        public static void PersonTypeSampleData(VisitPopDbContext context)
        {
            if (!context.PersonTypes.Any())
            {
                context.PersonTypes.Add(new AutoFaker<PersonType>());
                context.PersonTypes.Add(new AutoFaker<PersonType>());
                context.PersonTypes.Add(new AutoFaker<PersonType>());
                context.PersonTypes.Add(new AutoFaker<PersonType>());

                context.SaveChanges();
            }
        }
    }
}

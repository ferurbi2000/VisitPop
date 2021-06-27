using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class PersonSeeder
    {
        public static void PersonSampleData(VisitPopDbContext context)
        {
            if (!context.Persons.Any())
            {
                context.Persons.Add(new AutoFaker<Person>());
                context.Persons.Add(new AutoFaker<Person>());
                context.Persons.Add(new AutoFaker<Person>());
                context.Persons.Add(new AutoFaker<Person>());
                context.Persons.Add(new AutoFaker<Person>());

                context.SaveChanges();
            }
        }
    }
}

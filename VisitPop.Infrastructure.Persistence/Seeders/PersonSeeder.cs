using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class PersonSeeder
    {
        public static void SeedSamplePersonData(VisitPopDbContext context)
        {
            if (!context.People.Any())
            {
                //TODO: Revisar aqui sin AutoFaker puede añadir nuevos registros en español en las tablas                

                for (int i = 0; i < 100; i++)
                {
                    context.People.Add(new AutoFaker<Person>());
                }
                                
                context.SaveChanges();
            }
        }
    }
}

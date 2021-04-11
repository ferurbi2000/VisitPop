using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class PersonaSeeder
    {
        public static void PersonaSampleData(VisitPopDbContext context)
        {
            if (!context.Personas.Any())
            {
                context.Personas.Add(new AutoFaker<Persona>());
                context.Personas.Add(new AutoFaker<Persona>());
                context.Personas.Add(new AutoFaker<Persona>());
                context.Personas.Add(new AutoFaker<Persona>());
                context.Personas.Add(new AutoFaker<Persona>());

                context.SaveChanges();
            }
        }
    }
}

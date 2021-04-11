using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class VisitaPersonaSeeder
    {
        public static void VisitPersonaSampleData(VisitPopDbContext context)
        {
            if (!context.VisitaPersonas.Any())
            {
                context.VisitaPersonas.Add(new AutoFaker<VisitaPersona>());
                context.VisitaPersonas.Add(new AutoFaker<VisitaPersona>());
                context.VisitaPersonas.Add(new AutoFaker<VisitaPersona>());
                context.VisitaPersonas.Add(new AutoFaker<VisitaPersona>());
                context.VisitaPersonas.Add(new AutoFaker<VisitaPersona>());
                context.VisitaPersonas.Add(new AutoFaker<VisitaPersona>());

                context.SaveChanges();
            }
        }
    }
}

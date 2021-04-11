using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class VisitaSeeder
    {
        public static void SeedSampleVisitaData(VisitPopDbContext context)
        {
            if (!context.Visitas.Any())
            {
                context.Visitas.Add(new AutoFaker<Visita>());
                context.Visitas.Add(new AutoFaker<Visita>());
                context.Visitas.Add(new AutoFaker<Visita>());
                context.Visitas.Add(new AutoFaker<Visita>());
                context.Visitas.Add(new AutoFaker<Visita>());
                context.Visitas.Add(new AutoFaker<Visita>());
                context.Visitas.Add(new AutoFaker<Visita>());
                context.Visitas.Add(new AutoFaker<Visita>());
                context.Visitas.Add(new AutoFaker<Visita>());

                context.SaveChanges();
            }
        }
    }
}

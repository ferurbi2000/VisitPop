using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class ObservacionSeeder
    {
        public static void SeedSampleObservacionData(VisitPopDbContext context)
        {
            if (!context.Observaciones.Any())
            {
                context.Observaciones.Add(new AutoFaker<Observacion>());
                context.Observaciones.Add(new AutoFaker<Observacion>());
                context.Observaciones.Add(new AutoFaker<Observacion>());
                context.Observaciones.Add(new AutoFaker<Observacion>());

                context.SaveChanges();
            }
        }
    }
}

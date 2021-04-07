using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class EstadoVisitaSeeder
    {
        public static void SeedEstadoVisitaData(VisitPopDbContext context)
        {
            if (!context.EstadoVisitas.Any())
            {
                context.EstadoVisitas.Add(new AutoFaker<EstadoVisita>());
                context.EstadoVisitas.Add(new AutoFaker<EstadoVisita>());
                context.EstadoVisitas.Add(new AutoFaker<EstadoVisita>());

                context.SaveChanges();
            }
        }
    }
}

using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class PuntoControlSeeder
    {
        public static void SeedPuntoControlData(VisitPopDbContext context)
        {
            if (!context.PuntoControles.Any())
            {
                context.PuntoControles.Add(new AutoFaker<PuntoControl>());
                context.PuntoControles.Add(new AutoFaker<PuntoControl>());
                context.PuntoControles.Add(new AutoFaker<PuntoControl>());
                context.PuntoControles.Add(new AutoFaker<PuntoControl>());
                context.PuntoControles.Add(new AutoFaker<PuntoControl>());

                context.SaveChanges();
            }
        }
    }
}

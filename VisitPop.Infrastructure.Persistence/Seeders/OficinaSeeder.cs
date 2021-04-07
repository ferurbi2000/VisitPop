using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class OficinaSeeder
    {

        public static void SeedSampleOficinaData(VisitPopDbContext context)
        {
            if (!context.Oficinas.Any())
            {
                context.Oficinas.Add(new AutoFaker<Oficina>());
                context.Oficinas.Add(new AutoFaker<Oficina>());
                context.Oficinas.Add(new AutoFaker<Oficina>());
                context.Oficinas.Add(new AutoFaker<Oficina>());

                context.SaveChanges();
            }
        }
    }
}

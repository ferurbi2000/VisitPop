using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class TipoVisitaSeeder
    {
        public static void TipoVisitaSampleData(VisitPopDbContext context)
        {
            if (!context.TipoVisitas.Any())
            {
                context.TipoVisitas.Add(new AutoFaker<TipoVisita>());
                context.TipoVisitas.Add(new AutoFaker<TipoVisita>());
                context.TipoVisitas.Add(new AutoFaker<TipoVisita>());
                context.TipoVisitas.Add(new AutoFaker<TipoVisita>());

                context.SaveChanges();
            }
        }
    }
}

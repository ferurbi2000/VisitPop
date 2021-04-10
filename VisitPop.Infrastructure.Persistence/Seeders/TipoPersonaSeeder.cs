using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class TipoPersonaSeeder
    {
        public static void TipoPersonaSampleData(VisitPopDbContext context)
        {
            if (!context.TipoPersonas.Any())
            {
                context.TipoPersonas.Add(new AutoFaker<TipoPersona>());
                context.TipoPersonas.Add(new AutoFaker<TipoPersona>());
                context.TipoPersonas.Add(new AutoFaker<TipoPersona>());
                context.TipoPersonas.Add(new AutoFaker<TipoPersona>());

                context.SaveChanges();
            }
        }
    }
}

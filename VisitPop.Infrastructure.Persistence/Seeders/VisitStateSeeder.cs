using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class VisitStateSeeder
    {
        public static void SeedVisitStateData(VisitPopDbContext context)
        {
            if (!context.VisitStates.Any())
            {
                context.VisitStates.Add(new AutoFaker<VisitState>());
                context.VisitStates.Add(new AutoFaker<VisitState>());
                context.VisitStates.Add(new AutoFaker<VisitState>());

                context.SaveChanges();
            }
        }
    }
}

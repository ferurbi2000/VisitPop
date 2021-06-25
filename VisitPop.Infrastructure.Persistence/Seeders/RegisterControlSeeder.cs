using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class RegisterControlSeeder
    {
        public static void SeedRegisterControlData(VisitPopDbContext context)
        {
            if (!context.RegisterControls.Any())
            {
                context.RegisterControls.Add(new AutoFaker<RegisterControl>());
                context.RegisterControls.Add(new AutoFaker<RegisterControl>());
                context.RegisterControls.Add(new AutoFaker<RegisterControl>());
                context.RegisterControls.Add(new AutoFaker<RegisterControl>());
                context.RegisterControls.Add(new AutoFaker<RegisterControl>());

                context.SaveChanges();
            }
        }
    }
}

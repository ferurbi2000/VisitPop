using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class VehicleTypeSeeder
    {
        public static void VehicleTypeSampleData(VisitPopDbContext context)
        {
            if (!context.VehicleTypes.Any())
            {
                context.VehicleTypes.Add(new AutoFaker<VehicleType>());
                context.VehicleTypes.Add(new AutoFaker<VehicleType>());
                context.VehicleTypes.Add(new AutoFaker<VehicleType>());

                context.SaveChanges();
            }
        }
    }
}

using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class TipoVehiculoSeeder
    {
        public static void TipoVehiculoSampleData(VisitPopDbContext context)
        {
            if (!context.TipoVehiculos.Any())
            {
                context.TipoVehiculos.Add(new AutoFaker<TipoVehiculo>());
                context.TipoVehiculos.Add(new AutoFaker<TipoVehiculo>());
                context.TipoVehiculos.Add(new AutoFaker<TipoVehiculo>());

                context.SaveChanges();
            }
        }
    }
}

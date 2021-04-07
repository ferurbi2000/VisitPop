using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class EmpleadoSeeder
    {
        public static void SeedSampleEmpleadoData(VisitPopDbContext context)
        {
            if (!context.Empleados.Any())
            {
                context.Empleados.Add(new AutoFaker<Empleado>());
                context.Empleados.Add(new AutoFaker<Empleado>());
                context.Empleados.Add(new AutoFaker<Empleado>());
                context.Empleados.Add(new AutoFaker<Empleado>());
                context.Empleados.Add(new AutoFaker<Empleado>());
                context.Empleados.Add(new AutoFaker<Empleado>());
                context.Empleados.Add(new AutoFaker<Empleado>());
                context.Empleados.Add(new AutoFaker<Empleado>());
                context.Empleados.Add(new AutoFaker<Empleado>());

                context.SaveChanges();
            }
        }
    }
}

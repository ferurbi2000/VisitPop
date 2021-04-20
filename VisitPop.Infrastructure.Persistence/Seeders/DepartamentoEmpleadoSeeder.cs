using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class DepartamentoEmpleadoSeeder
    {
        public static void SeedSampleDepartamentoEmpleadoData(VisitPopDbContext context)
        {
            if (!context.DepartamentoEmpleados.Any())
            {

                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());
                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());
                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());
                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());
                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());
                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());
                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());
                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());
                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());
                context.DepartamentoEmpleados.Add(new AutoFaker<DepartamentoEmpleado>());

                context.SaveChanges();
            }
        }
    }
}

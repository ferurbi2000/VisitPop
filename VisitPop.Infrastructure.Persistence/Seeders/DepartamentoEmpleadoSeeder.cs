using AutoBogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                
                context.SaveChanges();
            }
        }
    }
}

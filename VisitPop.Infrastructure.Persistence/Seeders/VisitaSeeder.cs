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
    public static class VisitaSeeder
    {
        public static void SeedSampleVisitaData(VisitPopDbContext context)
        {
            if (!context.Visitas.Any())
            {
                context.Visitas.Add(new AutoFaker<Visita>());   
                context.Visitas.Add(new AutoFaker<Visita>());   
                context.Visitas.Add(new AutoFaker<Visita>());   
                context.Visitas.Add(new AutoFaker<Visita>());   
                context.Visitas.Add(new AutoFaker<Visita>());   
                context.Visitas.Add(new AutoFaker<Visita>());   
                context.Visitas.Add(new AutoFaker<Visita>());   
                context.Visitas.Add(new AutoFaker<Visita>());   
                context.Visitas.Add(new AutoFaker<Visita>());   

                context.SaveChanges();
            }
        }
    }
}

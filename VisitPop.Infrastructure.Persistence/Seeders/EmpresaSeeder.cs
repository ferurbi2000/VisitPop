using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class EmpresaSeeder
    {
        public static void SeedEmpresaData(VisitPopDbContext context)
        {
            if (!context.Empresas.Any())
            {
                context.Empresas.Add(new AutoFaker<Empresa>());
                context.Empresas.Add(new AutoFaker<Empresa>());
                context.Empresas.Add(new AutoFaker<Empresa>());
                context.Empresas.Add(new AutoFaker<Empresa>());

                context.SaveChanges();
            }
        }
    }
}

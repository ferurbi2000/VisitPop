using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class CompanySeeder
    {
        public static void SeedEmpresaData(VisitPopDbContext context)
        {
            if (!context.Companies.Any())
            {
                context.Companies.Add(new AutoFaker<Company>());
                context.Companies.Add(new AutoFaker<Company>());
                context.Companies.Add(new AutoFaker<Company>());
                context.Companies.Add(new AutoFaker<Company>());

                context.SaveChanges();
            }
        }
    }
}

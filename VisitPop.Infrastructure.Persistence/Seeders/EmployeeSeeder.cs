using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class EmployeeSeeder
    {
        public static void SeedSampleEmployeeData(VisitPopDbContext context)
        {
            if (!context.Employees.Any())
            {
                context.Employees.Add(new AutoFaker<Employee>());
                context.Employees.Add(new AutoFaker<Employee>());
                context.Employees.Add(new AutoFaker<Employee>());
                context.Employees.Add(new AutoFaker<Employee>());
                context.Employees.Add(new AutoFaker<Employee>());
                context.Employees.Add(new AutoFaker<Employee>());
                context.Employees.Add(new AutoFaker<Employee>());
                context.Employees.Add(new AutoFaker<Employee>());
                context.Employees.Add(new AutoFaker<Employee>());

                context.SaveChanges();
            }
        }
    }
}

using AutoBogus;
using System.Linq;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Seeders
{
    public static class EmployeeDepartmentSeeder
    {
        public static void SeedSampleEmployeeDepartmentData(VisitPopDbContext context)
        {
            if (!context.EmployeeDepartments.Any())
            {

                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());
                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());
                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());
                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());
                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());
                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());
                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());
                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());
                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());
                context.EmployeeDepartments.Add(new AutoFaker<EmployeeDepartment>());

                context.SaveChanges();
            }
        }
    }
}

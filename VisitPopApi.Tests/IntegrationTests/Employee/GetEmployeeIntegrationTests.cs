using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Responses;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPopApi.Tests.Fakes.Employee;
using VisitPopApi.Tests.Fakes.EmployeeDepartment;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.Employee
{
    [Collection("Sequential")]
    public class GetEmployeeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetEmployeeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetEmployee_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {


            var fakeEmployee = new FakeEmployeeDepartment().Ignore(d => d.Id).Generate();

            var fakeEmployeeOne = new FakeEmployee { }.Ignore(t => t.Id)
                .Ignore(d => d.EmployeeDepartmentId).Generate();
            //fakeEmpleadoOne.DepartamentoEmpleadoId = fakeDepartamento.Id;
            var fakeEmployeeTwo = new FakeEmployee { }.Ignore(t => t.Id)
                .Ignore(d => d.EmployeeDepartmentId).Generate();
            //fakeEmpleadoTwo.DepartamentoEmpleadoId = fakeDepartamento.Id;

            var appFactory = _factory;

            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                context.EmployeeDepartments.AddRange(fakeEmployee);
                context.SaveChanges();

                fakeEmployee = context.EmployeeDepartments.FirstOrDefault();
                fakeEmployeeOne.EmployeeDepartmentId = fakeEmployee.Id;
                fakeEmployeeOne.EmployeeDepartments = fakeEmployee;
                fakeEmployeeTwo.EmployeeDepartmentId = fakeEmployee.Id;
                fakeEmployeeTwo.EmployeeDepartments = fakeEmployee;

                //context.TipoPersonas.RemoveRange(context.TipoPersonas);
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo);

                context.SaveChanges();


            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/Employees")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<PageListEmployee>(responseContent).Employees;


            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeEmployeeOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeEmployeeTwo, options =>
                options.ExcludingMissingMembers());

        }
    }
}

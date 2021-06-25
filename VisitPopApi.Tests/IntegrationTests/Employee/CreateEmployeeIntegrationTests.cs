using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VisitPop.Application.Responses;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPopApi.Tests.Fakes.Employee;
using VisitPopApi.Tests.Fakes.EmployeeDepartment;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.Employee
{
    [Collection("Sequential")]
    public class CreateEmployeeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateEmployeeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostEmployeeReturnsSuccessCodeAndResourceWithAcurrateFields()
        {
            //Arrange

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var fakeDepartment = new FakeEmployeeDepartment().Ignore(d => d.Id).Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                //context.Empleados.RemoveRange(context.Empleados);
                context.EmployeeDepartments.AddRange(fakeDepartment);
                context.SaveChanges();

                fakeDepartment = context.EmployeeDepartments.FirstOrDefault();
            }

            var fakeEmployee = new FakeEmployeeDto().Ignore(d => d.Id).Ignore(d => d.EmployeeDepartmentId).Generate();
            fakeEmployee.EmployeeDepartmentId = fakeDepartment.Id;

            //Act
            var httpResponse = await client.PostAsJsonAsync("api/Employees", fakeEmployee)
                .ConfigureAwait(false);

            //Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var resultDto = JsonConvert.DeserializeObject<EmployeeResponseDto>(stringResponse).Employee;

            httpResponse.StatusCode.Should().Be(201);
            resultDto.FirstName.Should().Be(fakeEmployee.FirstName);
            resultDto.LastName.Should().Be(fakeEmployee.LastName);
            resultDto.DocId.Should().Be(fakeEmployee.DocId);
            resultDto.PhoneNumber.Should().Be(fakeEmployee.PhoneNumber);
            resultDto.EmployeeDepartmentId.Should().Be(fakeEmployee.EmployeeDepartmentId);
            resultDto.EmailAddress.Should().Be(fakeEmployee.EmailAddress);
            //resultDto.DepartamentoEmpleado.Should().Be(fakeEmpleado.DepartamentoEmpleado);

        }
    }
}

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VisitPop.Application.Responses;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPopApi.Tests.Fakes.EmployeeDepartment;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.DepartamentoEmpleado
{
    [Collection("Sequential")]
    public class GetEmployeeDepartmentIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetEmployeeDepartmentIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetDepartamentoEmpleados_ReturnsSuccessCodeAndResourceWithAcurrateFileds()
        {
            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Ignore(t => t.Id).Generate();
            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Ignore(t => t.Id).Generate();

            var appFactory = _factory;

            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                context.EmployeeDepartments.RemoveRange(context.EmployeeDepartments);
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo);

                context.SaveChanges();


            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/EmployeeDepartments")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<PageListEmployeeDepartment>(responseContent).EmployeeDepartments;


            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeEmployeeDepartmentOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeEmployeeDepartmentTwo, options =>
                options.ExcludingMissingMembers());
        }
    }
}

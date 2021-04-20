using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPopApi.Tests.Fakes.DepartamentoEmpleado;
using VisitPopApi.Tests.Responses;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.DepartamentoEmpleado
{
    [Collection("Sequential")]
    public class GetDepartamentoEmpleadoIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetDepartamentoEmpleadoIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetDepartamentoEmpleados_ReturnsSuccessCodeAndResourceWithAcurrateFileds()
        {
            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Ignore(t => t.Id).Generate();
            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Ignore(t => t.Id).Generate();

            var appFactory = _factory;

            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                context.DepartamentoEmpleados.RemoveRange(context.DepartamentoEmpleados);
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo);

                context.SaveChanges();


            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/DepartamentoEmpleados")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<PageListDepartamentoEmpleado>(responseContent).DepartamentoEmpleados;


            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoTwo, options =>
                options.ExcludingMissingMembers());
        }
    }
}

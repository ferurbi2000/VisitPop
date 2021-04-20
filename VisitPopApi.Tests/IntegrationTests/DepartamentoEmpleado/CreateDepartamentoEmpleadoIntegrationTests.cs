using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using VisitPopApi.Tests.Fakes.DepartamentoEmpleado;
using VisitPopApi.Tests.Responses;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.DepartamentoEmpleado
{
    [Collection("Sequential")]
    public class CreateDepartamentoEmpleadoIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateDepartamentoEmpleadoIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostDepartamentoEmpleadoReturnsSuccessCodeAndResourceWithAcurrateFields()
        {
            //Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeDepartamentoEmpleado = new FakeDepartamentoEmpleadoDto().Ignore(d => d.Id).Generate();

            //Act
            var httpResponse = await client.PostAsJsonAsync("api/DepartamentoEmpleados", fakeDepartamentoEmpleado)
                .ConfigureAwait(false);

            //Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var resultDto = JsonConvert.DeserializeObject<DepartamentoEmpleadoResponseDto>(stringResponse).DepartamentoEmpleado;

            httpResponse.StatusCode.Should().Be(201);
            resultDto.Nombre.Should().Be(fakeDepartamentoEmpleado.Nombre);

        }
    }
}

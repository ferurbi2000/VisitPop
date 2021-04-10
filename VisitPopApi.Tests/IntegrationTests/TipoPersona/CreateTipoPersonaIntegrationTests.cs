using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoPersona;
using VisitPopApi.Tests.Fakes.TipoPersona;
using VisitPopApi.Tests.Responses;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.TipoPersona
{
    [Collection("Sequential1")]
    public class CreateTipoPersonaIntegrationTests: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateTipoPersonaIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostTipoPersonaReturnSuccessCodeAndResourceWithAccurateFileds()
        {
            //  Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeTipoPersona = new FakeTipoPersonaDto().Ignore(t => t.Id).Generate();

            //  Act
            var httpResponse = await client.PostAsJsonAsync("api/TipoPersonas", fakeTipoPersona)             
                .ConfigureAwait(false);

            //  Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var resultDto = JsonConvert.DeserializeObject<TipoPersonaResponseDto>(stringResponse).TipoPersona;

            httpResponse.StatusCode.Should().Be(201);
            resultDto.Nombre.Should().Be(fakeTipoPersona.Nombre);

        }
    }
}

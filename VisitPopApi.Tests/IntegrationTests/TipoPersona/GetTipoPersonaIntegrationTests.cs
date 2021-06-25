using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoPersona;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPopApi.Tests.Fakes.TipoPersona;
using VisitPopApi.Tests.Responses;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.TipoPersona
{
    [Collection("Sequential")]
    public class GetTipoPersonaIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetTipoPersonaIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetTipoPersonas_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTipoPersonaOne = new FakeTipoPersona { }.Ignore(t => t.Id).Generate();
            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Ignore(t => t.Id).Generate();

            var appFactory = _factory;

            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                //context.TipoPersonas.RemoveRange(context.TipoPersonas);
                context.PersonTypes.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo);

                context.SaveChanges();


            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/TipoPersonas")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<PageListTipoPersona>(responseContent).TipoPersonas;


            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeTipoPersonaOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeTipoPersonaTwo, options =>
                options.ExcludingMissingMembers());

        }
    }
}

using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoPersona;
using VisitPop.Application.Mappings;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPopApi.Tests.Fakes.TipoPersona;
using VisitPopApi.Tests.Responses;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.TipoPersona
{
    public class UpdateTipoPersonaIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public UpdateTipoPersonaIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PutTipoPersonaReturnsBodyAndFieldsWereSuccessfullyUpdated()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TipoPersonaProfile>();
            }).CreateMapper();

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Ignore(t => t.Id).Generate();
            var expectedFinalObject = mapper.Map<TipoPersonaDto>(fakeTipoPersonaOne);
            expectedFinalObject.Nombre = "Easily Identified Value For Test";

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                //context.TipoPersonas.RemoveRange(context.TipoPersonas);
                context.TipoPersonas.AddRange(fakeTipoPersonaOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var serializedTipoPersonaToUpdate = JsonConvert.SerializeObject(expectedFinalObject);

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/TipoPersonas/?filters=Nombre=={fakeTipoPersonaOne.Nombre}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //var getResponse = JsonConvert.DeserializeObject<IEnumerable<TipoPersonaDto>>(getResponseContent);
            var getResponse = JsonConvert.DeserializeObject<PageListTipoPersona>(getResponseContent).TipoPersonas;

            var id = getResponse.FirstOrDefault().Id;
            expectedFinalObject.Id = id;

            //  put it
            var patchResult = await client.PutAsJsonAsync($"api/TipoPersonas/{id}", expectedFinalObject)
                .ConfigureAwait(false);

            //  get it again to confirm updates
            var checkResult = await client.GetAsync($"api/TipoPersonas/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            //var checkResponse = JsonConvert.DeserializeObject<TipoPersonaDto>(checkResponseContent);
            var checkResponse = JsonConvert.DeserializeObject<TipoPersonaResponseDto>(checkResponseContent).TipoPersona;


            // Assert
            patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
    }
}

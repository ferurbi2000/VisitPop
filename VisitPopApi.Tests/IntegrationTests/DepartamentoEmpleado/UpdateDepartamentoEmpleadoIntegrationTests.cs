using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.DepartamentoEmpleado;
using VisitPop.Application.Mappings;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPopApi.Tests.Fakes.DepartamentoEmpleado;
using VisitPopApi.Tests.Responses;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.DepartamentoEmpleado
{
    [Collection("Sequential")]
    public class UpdateDepartamentoEmpleadoIntegrationTests: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public UpdateDepartamentoEmpleadoIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task PatchDepartamentoEmpleado204AndFieldsWereSuccessfullyUpdated()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DepartamentoEmpleadoProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Ignore(d => d.Id).Generate();

            var expectedFinalObject = mapper.Map<DepartamentoEmpleadoDto>(fakeDepartamentoEmpleadoOne);
            expectedFinalObject.Nombre = lookupVal;

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                context.DepartamentoEmpleados.RemoveRange(context.DepartamentoEmpleados);
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var patchDoc = new JsonPatchDocument<DepartamentoEmpleadoForUpdateDto>();
            patchDoc.Replace(a => a.Nombre, lookupVal);
            var serializedDepartamentoEmpleadoToUpdate = JsonConvert.SerializeObject(patchDoc);

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/DepartamentoEmpleados/?filters=Nombre=={fakeDepartamentoEmpleadoOne.Nombre}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //var getResponse = JsonConvert.DeserializeObject<IEnumerable<DepartamentoEmpleadoDto>>(getResponseContent);
            var getResponse = JsonConvert.DeserializeObject<PageListDepartamentoEmpleado>(getResponseContent).DepartamentoEmpleados;

            var id = getResponse.FirstOrDefault().Id;
            expectedFinalObject.Id = id;

            // patch it
            var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/DepartamentoEmpleados/{id}")
            {
                Content = new StringContent(serializedDepartamentoEmpleadoToUpdate,
                    Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/DepartamentoEmpleados/{id}")
                .ConfigureAwait(false);

            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //var checkResponse = JsonConvert.DeserializeObject<DepartamentoEmpleadoDto>(checkResponseContent);
            var checkResponse = JsonConvert.DeserializeObject<DepartamentoEmpleadoResponseDto>(checkResponseContent).DepartamentoEmpleado;

            // Assert
            patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task PutDepartamentoEmpleadoReturnsBodyAndFieldsWereSuccessfullyUpdated()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DepartamentoEmpleadoProfile>();
            }).CreateMapper();

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Ignore(d => d.Id).Generate();
            var expectedFinalObject = mapper.Map<DepartamentoEmpleadoDto>(fakeDepartamentoEmpleadoOne);
            expectedFinalObject.Nombre = "Easily Identified Value For Test";

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                //context.DepartamentoEmpleados.RemoveRange(context.DepartamentoEmpleados);
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var serializedDepartamentoEmpleadoToUpdate = JsonConvert.SerializeObject(expectedFinalObject);

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/DepartamentoEmpleados/?filters=Nombre=={fakeDepartamentoEmpleadoOne.Nombre}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //var getResponse = JsonConvert.DeserializeObject<IEnumerable<DepartamentoEmpleadoDto>>(getResponseContent);
            var getResponse = JsonConvert.DeserializeObject<PageListDepartamentoEmpleado>(getResponseContent).DepartamentoEmpleados;

            var id = getResponse.FirstOrDefault().Id;
            expectedFinalObject.Id = id;

            // put it
            var patchResult = await client.PutAsJsonAsync($"api/DepartamentoEmpleados/{id}", expectedFinalObject)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/DepartamentoEmpleados/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //var checkResponse = JsonConvert.DeserializeObject<DepartamentoEmpleadoDto>(checkResponseContent);
            var checkResponse = JsonConvert.DeserializeObject<DepartamentoEmpleadoResponseDto>(checkResponseContent).DepartamentoEmpleado;

            // Assert
            patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
    }
}

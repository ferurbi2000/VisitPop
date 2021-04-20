using AutoMapper;
using FluentAssertions;
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
using VisitPopApi.Tests.Fakes.Empleado;
using VisitPopApi.Tests.Responses;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.Empleado
{
    [Collection("Sequential")]
    public class CreateEmpleadoIntegrationTests: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateEmpleadoIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostEmpleadoReturnsSuccessCodeAndResourceWithAcurrateFields()
        {
            //Arrange

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var fakeDepartamento = new FakeDepartamentoEmpleado().Ignore(d => d.Id).Generate();            

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                //context.Empleados.RemoveRange(context.Empleados);
                context.DepartamentoEmpleados.AddRange(fakeDepartamento);
                context.SaveChanges();
                           
                fakeDepartamento = context.DepartamentoEmpleados.FirstOrDefault();                
            }

            var fakeEmpleado = new FakeEmpleadoDto().Ignore(d => d.Id).Ignore(d => d.DepartamentoEmpleadoId).Generate();            
            fakeEmpleado.DepartamentoEmpleadoId = fakeDepartamento.Id;            

            //Act
            var httpResponse = await client.PostAsJsonAsync("api/Empleados", fakeEmpleado)
                .ConfigureAwait(false);

            //Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var resultDto = JsonConvert.DeserializeObject<EmpleadoResponseDto>(stringResponse).Empleado;

            httpResponse.StatusCode.Should().Be(201);
            resultDto.Nombres.Should().Be(fakeEmpleado.Nombres);
            resultDto.Apellidos.Should().Be(fakeEmpleado.Apellidos);
            resultDto.Identidad.Should().Be(fakeEmpleado.Identidad);
            resultDto.Telefono.Should().Be(fakeEmpleado.Telefono);
            resultDto.DepartamentoEmpleadoId.Should().Be(fakeEmpleado.DepartamentoEmpleadoId);
            resultDto.Email.Should().Be(fakeEmpleado.Email);
            //resultDto.DepartamentoEmpleado.Should().Be(fakeEmpleado.DepartamentoEmpleado);

        }
    }
}

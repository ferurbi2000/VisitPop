using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Mappings;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPopApi.Tests.Fakes.EmployeeDepartment;
using VisitPopApi.Tests.Fakes.Empleado;
using VisitPopApi.Tests.Responses;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.Empleado
{
    [Collection("Sequential")]
    public class GetEmpleadoIntegrationTests: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetEmpleadoIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetEmpleado_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {


            var fakeDepartamento = new FakeEmployeeDepartment().Ignore(d => d.Id).Generate();

            var fakeEmpleadoOne = new FakeEmpleado { }.Ignore(t => t.Id)
                .Ignore(d => d.EmployeeDepartmentId).Generate();
            //fakeEmpleadoOne.DepartamentoEmpleadoId = fakeDepartamento.Id;
            var fakeEmpleadoTwo = new FakeEmpleado { }.Ignore(t => t.Id)
                .Ignore(d => d.EmployeeDepartmentId).Generate();
            //fakeEmpleadoTwo.DepartamentoEmpleadoId = fakeDepartamento.Id;

            var appFactory = _factory;

            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                context.EmployeeDepartments.AddRange(fakeDepartamento);
                context.SaveChanges();

                fakeDepartamento = context.EmployeeDepartments.FirstOrDefault();
                fakeEmpleadoOne.EmployeeDepartmentId = fakeDepartamento.Id;
                fakeEmpleadoOne.EmployeeDepartments = fakeDepartamento;
                fakeEmpleadoTwo.EmployeeDepartmentId = fakeDepartamento.Id;
                fakeEmpleadoTwo.EmployeeDepartments = fakeDepartamento;

                //context.TipoPersonas.RemoveRange(context.TipoPersonas);
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo);

                context.SaveChanges();


            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/Empleados")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<PageListEmpleado>(responseContent).Empleados;


            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeEmpleadoOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeEmpleadoTwo, options =>
                options.ExcludingMissingMembers());

        }
    }
}

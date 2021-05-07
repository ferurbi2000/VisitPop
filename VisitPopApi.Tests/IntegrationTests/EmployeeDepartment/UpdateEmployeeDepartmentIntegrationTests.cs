using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Application.Mappings;
using VisitPop.Application.Responses;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPopApi.Tests.Fakes.EmployeeDepartment;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.DepartamentoEmpleado
{
    [Collection("Sequential")]
    public class UpdateEmployeeDepartmentIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public UpdateEmployeeDepartmentIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task PatchEmployeeDepartment204AndFieldsWereSuccessfullyUpdated()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EmployeeDepartmentProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Ignore(d => d.Id).Generate();

            var expectedFinalObject = mapper.Map<EmployeeDepartmentDto>(fakeEmployeeDepartmentOne);
            expectedFinalObject.Name = lookupVal;

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                context.EmployeeDepartments.RemoveRange(context.EmployeeDepartments);
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var patchDoc = new JsonPatchDocument<EmployeeDepartmentForUpdateDto>();
            patchDoc.Replace(a => a.Name, lookupVal);
            var serializedEmployeeDepartmentToUpdate = JsonConvert.SerializeObject(patchDoc);

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/EmployeeDepartments/?filters=Name=={fakeEmployeeDepartmentOne.Name}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //var getResponse = JsonConvert.DeserializeObject<IEnumerable<DepartamentoEmpleadoDto>>(getResponseContent);
            var getResponse = JsonConvert.DeserializeObject<PageListEmployeeDepartment>(getResponseContent).EmployeeDepartments;

            var id = getResponse.FirstOrDefault().Id;
            expectedFinalObject.Id = id;

            // patch it
            var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/EmployeeDepartments/{id}")
            {
                Content = new StringContent(serializedEmployeeDepartmentToUpdate,
                    Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/EmployeeDepartments/{id}")
                .ConfigureAwait(false);

            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //var checkResponse = JsonConvert.DeserializeObject<DepartamentoEmpleadoDto>(checkResponseContent);
            var checkResponse = JsonConvert.DeserializeObject<EmployeeDepartmentResponseDto>(checkResponseContent).EmployeeDepartment;

            // Assert
            patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task PutEmployeeDepartmentReturnsBodyAndFieldsWereSuccessfullyUpdated()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EmployeeDepartmentProfile>();
            }).CreateMapper();

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Ignore(d => d.Id).Generate();
            var expectedFinalObject = mapper.Map<EmployeeDepartmentDto>(fakeEmployeeDepartmentOne);
            expectedFinalObject.Name = "Easily Identified Value For Test";

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<VisitPopDbContext>();
                context.Database.EnsureCreated();

                //context.DepartamentoEmpleados.RemoveRange(context.DepartamentoEmpleados);
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var serializedEmployeeDepartmentToUpdate = JsonConvert.SerializeObject(expectedFinalObject);

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/EmployeeDepartments/?filters=Name=={fakeEmployeeDepartmentOne.Name}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //var getResponse = JsonConvert.DeserializeObject<IEnumerable<DepartamentoEmpleadoDto>>(getResponseContent);
            var getResponse = JsonConvert.DeserializeObject<PageListEmployeeDepartment>(getResponseContent).EmployeeDepartments;

            var id = getResponse.FirstOrDefault().Id;
            expectedFinalObject.Id = id;

            // put it
            var patchResult = await client.PutAsJsonAsync($"api/EmployeeDepartments/{id}", expectedFinalObject)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/EmployeeDepartments/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //var checkResponse = JsonConvert.DeserializeObject<DepartamentoEmpleadoDto>(checkResponseContent);
            var checkResponse = JsonConvert.DeserializeObject<EmployeeDepartmentResponseDto>(checkResponseContent).EmployeeDepartment;

            // Assert
            patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
    }
}

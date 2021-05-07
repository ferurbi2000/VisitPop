using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using VisitPop.Application.Responses;
using VisitPopApi.Tests.Fakes.EmployeeDepartment;
using Xunit;

namespace VisitPopApi.Tests.IntegrationTests.EmployeeDepartment
{
    [Collection("Sequential")]
    public class CreateEmployeeDepartmentIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateEmployeeDepartmentIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostEmployeeDepartmentReturnsSuccessCodeAndResourceWithAcurrateFields()
        {
            //Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeEmployeeDepartment = new FakeEmployeeDepartmentDto().Ignore(d => d.Id).Generate();

            //Act
            var httpResponse = await client.PostAsJsonAsync("api/EmployeeDepartments", fakeEmployeeDepartment)
                .ConfigureAwait(false);

            //Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var resultDto = JsonConvert.DeserializeObject<EmployeeDepartmentResponseDto>(stringResponse).EmployeeDepartment;

            httpResponse.StatusCode.Should().Be(201);
            resultDto.Name.Should().Be(fakeEmployeeDepartment.Name);

        }
    }
}

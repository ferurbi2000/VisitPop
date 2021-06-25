using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;
using VisitPopApi.Tests.Fakes.Employee;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.Employee
{
    [Collection("Sequential")]
    public class DeleteEmployeeRepositoryTests
    {
        [Fact]
        public void DeleteEmployee_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            var fakeEmployeeThree = new FakeEmployee { }.Generate();

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteEmployee(fakeEmployeeTwo);

                context.SaveChanges();

                //Assert
                var employeeList = context.Employees.ToList();

                employeeList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                employeeList.Should().ContainEquivalentOf(fakeEmployeeOne);
                employeeList.Should().ContainEquivalentOf(fakeEmployeeThree);
                Assert.DoesNotContain(employeeList, e => e == fakeEmployeeTwo);

                context.Database.EnsureDeleted();
            }
        }
    }
}

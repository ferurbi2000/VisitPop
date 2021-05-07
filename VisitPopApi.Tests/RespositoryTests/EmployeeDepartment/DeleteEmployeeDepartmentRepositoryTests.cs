using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;
using VisitPopApi.Tests.Fakes.EmployeeDepartment;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.DepartamentoEmpleado
{
    [Collection("Sequential")]
    public class DeleteEmployeeDepartmentRepositoryTests
    {
        [Fact]
        public void DeleteEmployeeDepartment_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Generate();
            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Generate();
            var fakeEmployeeDepartmentThree = new FakeEmployeeDepartment { }.Generate();

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentThree);

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteEmployeeDepartment(fakeEmployeeDepartmentTwo);

                context.SaveChanges();

                //Assert
                var EmployeeDepartmentList = context.EmployeeDepartments.ToList();

                EmployeeDepartmentList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                EmployeeDepartmentList.Should().ContainEquivalentOf(fakeEmployeeDepartmentOne);
                EmployeeDepartmentList.Should().ContainEquivalentOf(fakeEmployeeDepartmentThree);
                Assert.DoesNotContain(EmployeeDepartmentList, a => a == fakeEmployeeDepartmentTwo);

                context.Database.EnsureDeleted();
            }
        }
    }
}

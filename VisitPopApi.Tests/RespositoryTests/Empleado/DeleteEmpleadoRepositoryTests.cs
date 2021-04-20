using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;
using VisitPopApi.Tests.Fakes.Empleado;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.Empleado
{
    [Collection("Sequential")]
    public class DeleteEmpleadoRepositoryTests
    {
        [Fact]
        public void DeleteEmpleado_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteEmpleado(fakeEmpleadoTwo);

                context.SaveChanges();

                //Assert
                var empleadoList = context.Empleados.ToList();

                empleadoList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                empleadoList.Should().ContainEquivalentOf(fakeEmpleadoOne);
                empleadoList.Should().ContainEquivalentOf(fakeEmpleadoThree);
                Assert.DoesNotContain(empleadoList, e => e == fakeEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }
    }
}

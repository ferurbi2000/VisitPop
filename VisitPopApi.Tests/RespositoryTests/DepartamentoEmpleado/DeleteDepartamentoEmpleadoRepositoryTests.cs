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
using VisitPopApi.Tests.Fakes.DepartamentoEmpleado;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.DepartamentoEmpleado
{
    [Collection("Sequential")]
    public class DeleteDepartamentoEmpleadoRepositoryTests
    {
        [Fact]
        public void DeleteDepartamentoEmpleado_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Generate();
            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Generate();
            var fakeDepartamentoEmpleadoThree = new FakeDepartamentoEmpleado { }.Generate();

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoThree);

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteDepartamentoEmpleado(fakeDepartamentoEmpleadoTwo);

                context.SaveChanges();

                //Assert
                var DepartamentoEmpleadoList = context.DepartamentoEmpleados.ToList();

                DepartamentoEmpleadoList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                DepartamentoEmpleadoList.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoOne);
                DepartamentoEmpleadoList.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoThree);
                Assert.DoesNotContain(DepartamentoEmpleadoList, a => a == fakeDepartamentoEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }
    }
}

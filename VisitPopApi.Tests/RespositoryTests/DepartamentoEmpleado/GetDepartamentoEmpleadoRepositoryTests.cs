using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using VisitPop.Application.Dtos.DepartamentoEmpleado;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;
using VisitPopApi.Tests.Fakes.DepartamentoEmpleado;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.DepartamentoEmpleado
{
    [Collection("Sequential")]
    public class GetDepartamentoEmpleadoRepositoryTests
    {
        [Fact]
        public void GetDepartamentoEmpleado_ParametersMatchExpectedValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleado = new FakeDepartamentoEmpleado { }.Generate();

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleado);
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                //Assert
                var DepartamentoEmpleadoById = service.GetDepartamentoEmpleado(fakeDepartamentoEmpleado.Id);
                DepartamentoEmpleadoById.Id.Should().Be(fakeDepartamentoEmpleado.Id);
                DepartamentoEmpleadoById.Nombre.Should().Be(fakeDepartamentoEmpleado.Nombre);
            }
        }

        [Fact]
        public async void GetDepartamentoEmpleadosAsync_CountMatchesAndContainsEquivalentObjects()
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
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var DepartamentoEmpleadoRepo = await service.GetDepartamentoEmpleadosAsync(new DepartamentoEmpleadoParametersDto());

                //Assert
                DepartamentoEmpleadoRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                DepartamentoEmpleadoRepo.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoOne);
                DepartamentoEmpleadoRepo.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoTwo);
                DepartamentoEmpleadoRepo.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetDepartamentoEmpleadosAsync_ReturnExpectedPageSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Ignore(d => d.Id).Generate();
            fakeDepartamentoEmpleadoOne.Id = 1;
            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Ignore(d => d.Id).Generate();
            fakeDepartamentoEmpleadoTwo.Id = 2;
            var fakeDepartamentoEmpleadoThree = new FakeDepartamentoEmpleado { }.Ignore(d => d.Id).Generate();
            fakeDepartamentoEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoThree);
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var DepartamentoEmpleadoRepo = await service.GetDepartamentoEmpleadosAsync(new DepartamentoEmpleadoParametersDto { PageSize = 2 });

                //Assert
                DepartamentoEmpleadoRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                DepartamentoEmpleadoRepo.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoOne);
                DepartamentoEmpleadoRepo.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetDepartamentoEmpleadosAsync_ReturnExpectedPageNumberAndSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Ignore(d => d.Id).Generate();
            fakeDepartamentoEmpleadoOne.Id = 1;
            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Ignore(d => d.Id).Generate();
            fakeDepartamentoEmpleadoTwo.Id = 2;
            var fakeDepartamentoEmpleadoThree = new FakeDepartamentoEmpleado { }.Ignore(d => d.Id).Generate();
            fakeDepartamentoEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoThree);
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var DepartamentoEmpleadoRepo = await service.GetDepartamentoEmpleadosAsync(new DepartamentoEmpleadoParametersDto { PageSize = 1, PageNumber = 2 });

                //Assert
                DepartamentoEmpleadoRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                DepartamentoEmpleadoRepo.Should().ContainEquivalentOf(fakeDepartamentoEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetDepartamentoEmpleadosAsync_ListDepartamentoEmpleadoIdSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoOne.Id = 2;

            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoTwo.Id = 1;

            var fakeDepartamentoEmpleadoThree = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoThree);
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var DepartamentoEmpleadoRepo = await service.GetDepartamentoEmpleadosAsync(new DepartamentoEmpleadoParametersDto { SortOrder = "Id" });

                //Assert
                DepartamentoEmpleadoRepo.Should()
                    .ContainInOrder(fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetDepartamentoEmpleadosAsync_ListDepartamentoEmpleadoIdSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoOne.Id = 2;

            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoTwo.Id = 1;

            var fakeDepartamentoEmpleadoThree = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoThree);
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var DepartamentoEmpleadoRepo = await service.GetDepartamentoEmpleadosAsync(new DepartamentoEmpleadoParametersDto { SortOrder = "-Id" });

                //Assert
                DepartamentoEmpleadoRepo.Should()
                    .ContainInOrder(fakeDepartamentoEmpleadoThree, fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetDepartamentoEmpleadosAsync_ListNombreSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoOne.Nombre = "bravo";

            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoTwo.Nombre = "alpha";

            var fakeDepartamentoEmpleadoThree = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoThree.Nombre = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoThree);
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var DepartamentoEmpleadoRepo = await service.GetDepartamentoEmpleadosAsync(new DepartamentoEmpleadoParametersDto { SortOrder = "Nombre" });

                //Assert
                DepartamentoEmpleadoRepo.Should()
                    .ContainInOrder(fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetDepartamentoEmpleadosAsync_ListNombreSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoOne.Nombre = "bravo";

            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoTwo.Nombre = "alpha";

            var fakeDepartamentoEmpleadoThree = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoThree.Nombre = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoThree);
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var DepartamentoEmpleadoRepo = await service.GetDepartamentoEmpleadosAsync(new DepartamentoEmpleadoParametersDto { SortOrder = "-Nombre" });

                //Assert
                DepartamentoEmpleadoRepo.Should()
                    .ContainInOrder(fakeDepartamentoEmpleadoThree, fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }


        [Fact]
        public async void GetDepartamentoEmpleadosAsync_FilterDepartamentoEmpleadoIdListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoOne.Id = 1;

            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoTwo.Id = 2;

            var fakeDepartamentoEmpleadoThree = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoThree);
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var DepartamentoEmpleadoRepo = await service.GetDepartamentoEmpleadosAsync(new DepartamentoEmpleadoParametersDto { Filters = $"Id == {fakeDepartamentoEmpleadoTwo.Id}" });

                //Assert
                DepartamentoEmpleadoRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetDepartamentoEmpleadosAsync_FilterNombreListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"DepartamentoEmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeDepartamentoEmpleadoOne = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoOne.Nombre = "alpha";

            var fakeDepartamentoEmpleadoTwo = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoTwo.Nombre = "bravo";

            var fakeDepartamentoEmpleadoThree = new FakeDepartamentoEmpleado { }.Generate();
            fakeDepartamentoEmpleadoThree.Nombre = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.DepartamentoEmpleados.AddRange(fakeDepartamentoEmpleadoOne, fakeDepartamentoEmpleadoTwo, fakeDepartamentoEmpleadoThree);
                context.SaveChanges();

                var service = new DepartamentoEmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var DepartamentoEmpleadoRepo = await service.GetDepartamentoEmpleadosAsync(new DepartamentoEmpleadoParametersDto { Filters = $"Nombre == {fakeDepartamentoEmpleadoTwo.Nombre}" });

                //Assert
                DepartamentoEmpleadoRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }
    }
}

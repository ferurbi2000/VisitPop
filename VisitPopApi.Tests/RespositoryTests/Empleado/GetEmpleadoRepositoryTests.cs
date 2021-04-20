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
using VisitPop.Application.Dtos.Empleado;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;
using VisitPopApi.Tests.Fakes.Empleado;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.Empleado
{
    [Collection("Sequential")]
    public class GetEmpleadoRepositoryTests
    {
        [Fact]
        public void GetEmpleado_ParametersMatchExpectedValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleado = new FakeEmpleado { }.Generate();

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleado);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                //Assert
                var empleadoById = service.GetEmpleado(fakeEmpleado.Id);
                empleadoById.Id.Should().Be(fakeEmpleado.Id);
                empleadoById.Nombres.Should().Be(fakeEmpleado.Nombres);
                empleadoById.Apellidos.Should().Be(fakeEmpleado.Apellidos);
                empleadoById.Identidad.Should().Be(fakeEmpleado.Identidad);
                empleadoById.Telefono.Should().Be(fakeEmpleado.Telefono);
                empleadoById.DepartamentoEmpleadoId.Should().Be(fakeEmpleado.DepartamentoEmpleadoId);
                empleadoById.Email.Should().Be(fakeEmpleado.Email);
                empleadoById.DepartamentoEmpleado.Should().Be(fakeEmpleado.DepartamentoEmpleado);
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_CountMatchesAndContainsEquivalentObjects()
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
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto());

                //Assert
                empleadoRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                empleadoRepo.Should().ContainEquivalentOf(fakeEmpleadoOne);
                empleadoRepo.Should().ContainEquivalentOf(fakeEmpleadoTwo);
                empleadoRepo.Should().ContainEquivalentOf(fakeEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ReturnExpectedPageSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Ignore(e=>e.Id).Generate();
            fakeEmpleadoOne.Id = 1;
            var fakeEmpleadoTwo = new FakeEmpleado { }.Ignore(e => e.Id).Generate();
            fakeEmpleadoTwo.Id = 2;
            var fakeEmpleadoThree = new FakeEmpleado { }.Ignore(e => e.Id).Generate();
            fakeEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { PageSize = 2 });

                //Assert
                empleadoRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                empleadoRepo.Should().ContainEquivalentOf(fakeEmpleadoOne);
                empleadoRepo.Should().ContainEquivalentOf(fakeEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ReturnExpectedPageNumberAndSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Ignore(e => e.Id).Generate();
            fakeEmpleadoOne.Id = 1;
            var fakeEmpleadoTwo = new FakeEmpleado { }.Ignore(e => e.Id).Generate();
            fakeEmpleadoTwo.Id = 2;
            var fakeEmpleadoThree = new FakeEmpleado { }.Ignore(e => e.Id).Generate();
            fakeEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { PageSize = 1, PageNumber = 2 });

                //Assert
                empleadoRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                empleadoRepo.Should().ContainEquivalentOf(fakeEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListEmpleadoIdSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Id = 2;

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Id = 1;

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "Id" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoTwo, fakeEmpleadoOne, fakeEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListEmpleadoIdSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Id = 2;

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Id = 1;

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "-Id" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoThree, fakeEmpleadoOne, fakeEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListNombresSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Nombres = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Nombres = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Nombres = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "Nombres" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoTwo, fakeEmpleadoOne, fakeEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListNombresSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Nombres = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Nombres = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Nombres = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "-Nombres" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoThree, fakeEmpleadoOne, fakeEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListApellidosSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Apellidos = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Apellidos = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Apellidos = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "Apellidos" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoTwo, fakeEmpleadoOne, fakeEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListApellidosSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Apellidos = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Apellidos = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Apellidos = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "-Apellidos" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoThree, fakeEmpleadoOne, fakeEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListIdentidadSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Identidad = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Identidad = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Identidad = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "Identidad" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoTwo, fakeEmpleadoOne, fakeEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListIdentidadSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Identidad = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Identidad = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Identidad = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "-Identidad" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoThree, fakeEmpleadoOne, fakeEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListTelefonoSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Telefono = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Telefono = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Telefono = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "Telefono" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoTwo, fakeEmpleadoOne, fakeEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListTelefonoSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Telefono = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Telefono = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Telefono = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "-Telefono" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoThree, fakeEmpleadoOne, fakeEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListEmailSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Email = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Email = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Email = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "Email" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoTwo, fakeEmpleadoOne, fakeEmpleadoThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_ListEmailSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Email = "bravo";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Email = "alpha";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Email = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { SortOrder = "-Email" });

                //Assert
                empleadoRepo.Should()
                    .ContainInOrder(fakeEmpleadoThree, fakeEmpleadoOne, fakeEmpleadoTwo);

                context.Database.EnsureDeleted();
            }
        }


        [Fact]
        public async void GetEmpleadosAsync_FilterEmpleadoIdListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Id = 1;

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Id = 2;

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { Filters = $"Id == {fakeEmpleadoTwo.Id}" });

                //Assert
                empleadoRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_FilterNombresListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Nombres = "alpha";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Nombres = "bravo";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Nombres = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { Filters = $"Nombres == {fakeEmpleadoTwo.Nombres}" });

                //Assert
                empleadoRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_FilterApellidosListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Apellidos = "alpha";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Apellidos = "bravo";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Apellidos = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { Filters = $"Apellidos == {fakeEmpleadoTwo.Apellidos}" });

                //Assert
                empleadoRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_FilterIdentidadListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Identidad = "alpha";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Identidad = "bravo";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Identidad = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { Filters = $"Identidad == {fakeEmpleadoTwo.Identidad}" });

                //Assert
                empleadoRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_FilterTelefonoListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Telefono = "alpha";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Telefono = "bravo";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Telefono = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { Filters = $"Telefono == {fakeEmpleadoTwo.Telefono}" });

                //Assert
                empleadoRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_FilterDepartamentoEmpleadoIdListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.DepartamentoEmpleadoId = 1;

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.DepartamentoEmpleadoId = 2;

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.DepartamentoEmpleadoId = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { Filters = $"DepartamentoEmpleadoId == {fakeEmpleadoTwo.DepartamentoEmpleadoId}" });

                //Assert
                empleadoRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmpleadosAsync_FilterEmailListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmpleadoOne = new FakeEmpleado { }.Generate();
            fakeEmpleadoOne.Email = "alpha";

            var fakeEmpleadoTwo = new FakeEmpleado { }.Generate();
            fakeEmpleadoTwo.Email = "bravo";

            var fakeEmpleadoThree = new FakeEmpleado { }.Generate();
            fakeEmpleadoThree.Email = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Empleados.AddRange(fakeEmpleadoOne, fakeEmpleadoTwo, fakeEmpleadoThree);
                context.SaveChanges();

                var service = new EmpleadoRepository(context, new SieveProcessor(sieveOptions));

                var empleadoRepo = await service.GetEmpleadosAsync(new EmpleadoParametersDto { Filters = $"Email == {fakeEmpleadoTwo.Email}" });

                //Assert
                empleadoRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }
    }
}

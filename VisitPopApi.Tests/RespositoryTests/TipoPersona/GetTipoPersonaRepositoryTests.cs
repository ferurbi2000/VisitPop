using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using VisitPop.Application.Dtos.TipoPersona;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;
using VisitPopApi.Tests.Fakes.TipoPersona;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.TipoPersona
{
    [Collection("Sequential")]
    public class GetTipoPersonaRepositoryTests
    {
        [Fact]
        public void GetTipoPersona_ParamatersMatchExpectedValues()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"VisitPopDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersona = new FakeTipoPersona { }.Generate();

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersona);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));

                //  Assert
                var tipoPersonaById = service.GetTipoPersona(fakeTipoPersona.Id);
                tipoPersonaById.Id.Should().Be(fakeTipoPersona.Id);
                tipoPersonaById.Nombre.Should().Be(fakeTipoPersona.Nombre);
            }
        }

        [Fact]
        public async void GetTipoPersonaAsync_CountMatchesAndCointainsEquivalentObjects()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Generate();
            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Generate();
            var fakeTipoPersonaThree = new FakeTipoPersona { }.Generate();

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo, fakeTipoPersonaThree);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));

                var tipoPersonaRepo = await service.GetTipoPersonas(new TipoPersonaParametersDto());

                //  Assert
                tipoPersonaRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                tipoPersonaRepo.Should().ContainEquivalentOf(fakeTipoPersonaOne);
                tipoPersonaRepo.Should().ContainEquivalentOf(fakeTipoPersonaTwo);
                tipoPersonaRepo.Should().ContainEquivalentOf(fakeTipoPersonaThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTipoPersonaAsync_ReturnExpectedPageSize()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptios = Options.Create(new SieveOptions());

            var fakeTipoPeronaOne = new FakeTipoPersona { }.Ignore(t => t.Id).Generate();
            fakeTipoPeronaOne.Id = 1;
            var fakeTipoPeronaTwo = new FakeTipoPersona { }.Ignore(t => t.Id).Generate();
            fakeTipoPeronaTwo.Id = 2;
            var fakeTipoPeronaThree = new FakeTipoPersona { }.Ignore(t => t.Id).Generate();
            fakeTipoPeronaThree.Id = 3;

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPeronaOne, fakeTipoPeronaTwo, fakeTipoPeronaThree);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptios));

                var tipoPersonaRepo = await service.GetTipoPersonas(new TipoPersonaParametersDto { PageSize = 2 });

                //  Assert
                tipoPersonaRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                tipoPersonaRepo.Should().ContainEquivalentOf(fakeTipoPeronaOne);
                tipoPersonaRepo.Should().ContainEquivalentOf(fakeTipoPeronaTwo);

                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public async void GetTipoPersonasAsync_ReturnExpectedPageNumberAndSize()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Ignore(t => t.Id).Generate();
            fakeTipoPersonaOne.Id = 1;
            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Ignore(t => t.Id).Generate();
            fakeTipoPersonaTwo.Id = 2;
            var fakeTipoPersonaThree = new FakeTipoPersona { }.Ignore(t => t.Id).Generate();
            fakeTipoPersonaThree.Id = 3;

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo, fakeTipoPersonaThree);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));

                var tipoPersonaRepo = await service.GetTipoPersonas(new TipoPersonaParametersDto { PageSize = 1, PageNumber = 2 });

                //  Assert
                tipoPersonaRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                tipoPersonaRepo.Should().ContainEquivalentOf(fakeTipoPersonaTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTipoPersonasAsync_ListTipoPersonaIdSortedInAscOrder()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaOne.Id = 2;

            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaTwo.Id = 1;

            var fakeTipoPersonaThree = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaThree.Id = 3;

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo, fakeTipoPersonaThree);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));

                var tipoPersonaRepo = await service.GetTipoPersonas(new TipoPersonaParametersDto { SortOrder = "Id" });

                //  Assert
                tipoPersonaRepo.Should()
                    .ContainInOrder(fakeTipoPersonaTwo, fakeTipoPersonaOne, fakeTipoPersonaThree);

                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public async void GetTipoPersonasAsync_ListTipoPersonaIdSortedInDescOrder()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaOne.Id = 2;

            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaTwo.Id = 1;

            var fakeTipoPersonaThree = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaThree.Id = 3;

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo, fakeTipoPersonaThree);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));

                var tipoPersonaRepo = await service.GetTipoPersonas(new TipoPersonaParametersDto { SortOrder = "-Id" });

                //  Assert
                tipoPersonaRepo.Should()
                    .ContainInOrder(fakeTipoPersonaThree, fakeTipoPersonaOne, fakeTipoPersonaTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTipoPersonasAsync_ListNombreSortedInAscOrder()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaOne.Nombre = "bravo";

            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaTwo.Nombre = "alpha";

            var fakeTipoPersonaThree = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaThree.Nombre = "charlie";

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo, fakeTipoPersonaThree);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));

                var tipoPersonaRepo = await service.GetTipoPersonas(new TipoPersonaParametersDto { SortOrder = "Nombre" });

                //  Assert
                tipoPersonaRepo.Should()
                    .ContainInOrder(fakeTipoPersonaTwo, fakeTipoPersonaOne, fakeTipoPersonaThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTipoPersonasAsync_ListNombreSortedInDescOrder()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaOne.Nombre = "bravo";

            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaTwo.Nombre = "alpha";

            var fakeTipoPersonaThree = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaThree.Nombre = "charlie";

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo, fakeTipoPersonaThree);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));

                var tipoPersonaRepo = await service.GetTipoPersonas(new TipoPersonaParametersDto { SortOrder = "-Nombre" });

                //  Assert
                tipoPersonaRepo.Should()
                    .ContainInOrder(fakeTipoPersonaThree, fakeTipoPersonaOne, fakeTipoPersonaTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTipoPersonasAsync_FilterTipoPersonaIdListWithExact()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaOne.Id = 1;

            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaTwo.Id = 2;

            var fakeTipoPersonaThree = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaThree.Id = 3;

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo, fakeTipoPersonaThree);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));

                var tipoPersonaRepo = await service.GetTipoPersonas(new TipoPersonaParametersDto { Filters = $"Id == {fakeTipoPersonaTwo.Id}" });

                //  Assert
                tipoPersonaRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTipoPersonasAsync_FilterNombreListWithExact()
        {
            //  Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaOne.Nombre = "alpha";

            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaTwo.Nombre = "bravo";

            var fakeTipoPersonaThree = new FakeTipoPersona { }.Generate();
            fakeTipoPersonaThree.Nombre = "charlie";

            //  Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo, fakeTipoPersonaThree);
                context.SaveChanges();

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));

                var tipoPersonaRepo = await service.GetTipoPersonas(new TipoPersonaParametersDto { Filters = $"Nombre == {fakeTipoPersonaTwo.Nombre}" });

                //  Assert
                tipoPersonaRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }
    }
}

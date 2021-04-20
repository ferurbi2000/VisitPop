using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;
using VisitPopApi.Tests.Fakes.TipoPersona;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.TipoPersona
{
    [Collection("Sequential")]
    public class DeleteTipoPersonaRepositoryTests
    {
        [Fact]
        public void DeleteTipoPersona_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"TipoPersonaDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTipoPersonaOne = new FakeTipoPersona { }.Generate();
            var fakeTipoPersonaTwo = new FakeTipoPersona { }.Generate();
            var fakeTipoPersonaThree = new FakeTipoPersona { }.Generate();

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.TipoPersonas.AddRange(fakeTipoPersonaOne, fakeTipoPersonaTwo, fakeTipoPersonaThree);

                var service = new TipoPersonaRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteTipoPersona(fakeTipoPersonaTwo);

                context.SaveChanges();

                //Assert
                var TipoPersonList = context.TipoPersonas.ToList();

                TipoPersonList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                TipoPersonList.Should().ContainEquivalentOf(fakeTipoPersonaOne);
                TipoPersonList.Should().ContainEquivalentOf(fakeTipoPersonaThree);
                Assert.DoesNotContain(TipoPersonList, a => a == fakeTipoPersonaTwo);

                context.Database.EnsureDeleted();
            }
        }
    }
}

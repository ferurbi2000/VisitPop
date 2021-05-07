using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;
using VisitPopApi.Tests.Fakes.EmployeeDepartment;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.EmployeeDepartment
{
    [Collection("Sequential")]
    public class GetEmployeeDepartmentRepositoryTests
    {
        [Fact]
        public void GetEmployeeDepartment_ParametersMatchExpectedValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartment = new FakeEmployeeDepartment { }.Generate();

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartment);
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                //Assert
                var EmployeeDepartmentById = service.GetEmployeeDepartment(fakeEmployeeDepartment.Id);
                EmployeeDepartmentById.Id.Should().Be(fakeEmployeeDepartment.Id);
                EmployeeDepartmentById.Name.Should().Be(fakeEmployeeDepartment.Name);
            }
        }

        [Fact]
        public async void GetEmployeeDepartmentsAsync_CountMatchesAndContainsEquivalentObjects()
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
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                var EmployeeDepartmentRepo = await service.GetEmployeeDepartmentsAsync(new EmployeeDepartmentParametersDto());

                //Assert
                EmployeeDepartmentRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                EmployeeDepartmentRepo.Should().ContainEquivalentOf(fakeEmployeeDepartmentOne);
                EmployeeDepartmentRepo.Should().ContainEquivalentOf(fakeEmployeeDepartmentTwo);
                EmployeeDepartmentRepo.Should().ContainEquivalentOf(fakeEmployeeDepartmentThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeeDepartmentsAsync_ReturnExpectedPageSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Ignore(d => d.Id).Generate();
            fakeEmployeeDepartmentOne.Id = 1;
            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Ignore(d => d.Id).Generate();
            fakeEmployeeDepartmentTwo.Id = 2;
            var fakeEmployeeDepartmentThree = new FakeEmployeeDepartment { }.Ignore(d => d.Id).Generate();
            fakeEmployeeDepartmentThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentThree);
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                var EmployeeDepartmentRepo = await service.GetEmployeeDepartmentsAsync(new EmployeeDepartmentParametersDto { PageSize = 2 });

                //Assert
                EmployeeDepartmentRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                EmployeeDepartmentRepo.Should().ContainEquivalentOf(fakeEmployeeDepartmentOne);
                EmployeeDepartmentRepo.Should().ContainEquivalentOf(fakeEmployeeDepartmentTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeeDepartmentsAsync_ReturnExpectedPageNumberAndSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Ignore(d => d.Id).Generate();
            fakeEmployeeDepartmentOne.Id = 1;
            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Ignore(d => d.Id).Generate();
            fakeEmployeeDepartmentTwo.Id = 2;
            var fakeEmployeeDepartmentThree = new FakeEmployeeDepartment { }.Ignore(d => d.Id).Generate();
            fakeEmployeeDepartmentThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentThree);
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                var EmployeeDepartmentRepo = await service.GetEmployeeDepartmentsAsync(new EmployeeDepartmentParametersDto { PageSize = 1, PageNumber = 2 });

                //Assert
                EmployeeDepartmentRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                EmployeeDepartmentRepo.Should().ContainEquivalentOf(fakeEmployeeDepartmentTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeeDepartmentsAsync_ListEmployeeDepartmentIdSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentOne.Id = 2;

            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentTwo.Id = 1;

            var fakeEmployeeDepartmentThree = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentThree);
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                var EmployeeDepartmentRepo = await service.GetEmployeeDepartmentsAsync(new EmployeeDepartmentParametersDto { SortOrder = "Id" });

                //Assert
                EmployeeDepartmentRepo.Should()
                    .ContainInOrder(fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentOne, fakeEmployeeDepartmentThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeeDepartmentsAsync_ListEmployeeDepartmentIdSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentOne.Id = 2;

            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentTwo.Id = 1;

            var fakeEmployeeDepartmentThree = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentThree);
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                var EmployeeDepartmentRepo = await service.GetEmployeeDepartmentsAsync(new EmployeeDepartmentParametersDto { SortOrder = "-Id" });

                //Assert
                EmployeeDepartmentRepo.Should()
                    .ContainInOrder(fakeEmployeeDepartmentThree, fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeeDepartmentsAsync_ListNameSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentOne.Name = "bravo";

            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentTwo.Name = "alpha";

            var fakeEmployeeDepartmentThree = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentThree.Name = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentThree);
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                var EmployeeDepartmentRepo = await service.GetEmployeeDepartmentsAsync(new EmployeeDepartmentParametersDto { SortOrder = "Name" });

                //Assert
                EmployeeDepartmentRepo.Should()
                    .ContainInOrder(fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentOne, fakeEmployeeDepartmentThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeeDepartmentsAsync_ListNameSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentOne.Name = "bravo";

            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentTwo.Name = "alpha";

            var fakeEmployeeDepartmentThree = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentThree.Name = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentThree);
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                var EmployeeDepartmentRepo = await service.GetEmployeeDepartmentsAsync(new EmployeeDepartmentParametersDto { SortOrder = "-Name" });

                //Assert
                EmployeeDepartmentRepo.Should()
                    .ContainInOrder(fakeEmployeeDepartmentThree, fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo);

                context.Database.EnsureDeleted();
            }
        }


        [Fact]
        public async void GetEmployeeDepartmentsAsync_FilterEmployeeDepartmentIdListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentOne.Id = 1;

            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentTwo.Id = 2;

            var fakeEmployeeDepartmentThree = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentThree);
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                var EmployeeDepartmentRepo = await service.GetEmployeeDepartmentsAsync(new EmployeeDepartmentParametersDto { Filters = $"Id == {fakeEmployeeDepartmentTwo.Id}" });

                //Assert
                EmployeeDepartmentRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeeDepartmentsAsync_FilterNameListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDepartmentDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeDepartmentOne = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentOne.Name = "alpha";

            var fakeEmployeeDepartmentTwo = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentTwo.Name = "bravo";

            var fakeEmployeeDepartmentThree = new FakeEmployeeDepartment { }.Generate();
            fakeEmployeeDepartmentThree.Name = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.EmployeeDepartments.AddRange(fakeEmployeeDepartmentOne, fakeEmployeeDepartmentTwo, fakeEmployeeDepartmentThree);
                context.SaveChanges();

                var service = new EmployeeDepartmentRepository(context, new SieveProcessor(sieveOptions));

                var EmployeeDepartmentRepo = await service.GetEmployeeDepartmentsAsync(new EmployeeDepartmentParametersDto { Filters = $"Name == {fakeEmployeeDepartmentTwo.Name}" });

                //Assert
                EmployeeDepartmentRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }
    }
}

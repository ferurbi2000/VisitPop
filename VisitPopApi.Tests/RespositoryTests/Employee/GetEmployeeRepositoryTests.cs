using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;
using VisitPopApi.Tests.Fakes.Employee;
using Xunit;

namespace VisitPopApi.Tests.RespositoryTests.Employee
{
    [Collection("Sequential")]
    public class GetEmployeeRepositoryTests
    {
        [Fact]
        public void GetEmployee_ParametersMatchExpectedValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployee = new FakeEmployee { }.Generate();

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployee);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                //Assert
                var empleadoById = service.GetEmployee(fakeEmployee.Id);
                empleadoById.Id.Should().Be(fakeEmployee.Id);
                empleadoById.FirstName.Should().Be(fakeEmployee.FirstName);
                empleadoById.LastName.Should().Be(fakeEmployee.LastName);
                empleadoById.DocId.Should().Be(fakeEmployee.DocId);
                empleadoById.PhoneNumber.Should().Be(fakeEmployee.PhoneNumber);
                empleadoById.EmployeeDepartmentId.Should().Be(fakeEmployee.EmployeeDepartmentId);
                empleadoById.EmailAddress.Should().Be(fakeEmployee.EmailAddress);
                empleadoById.EmployeeDepartments.Should().Be(fakeEmployee.EmployeeDepartments);
            }
        }

        [Fact]
        public async void GetEmployeesAsync_CountMatchesAndContainsEquivalentObjects()
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
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto());

                //Assert
                employeeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                employeeRepo.Should().ContainEquivalentOf(fakeEmployeeOne);
                employeeRepo.Should().ContainEquivalentOf(fakeEmployeeTwo);
                employeeRepo.Should().ContainEquivalentOf(fakeEmployeeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ReturnExpectedPageSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Ignore(e => e.Id).Generate();
            fakeEmployeeOne.Id = 1;
            var fakeEmployeeTwo = new FakeEmployee { }.Ignore(e => e.Id).Generate();
            fakeEmployeeTwo.Id = 2;
            var fakeEmployeeThree = new FakeEmployee { }.Ignore(e => e.Id).Generate();
            fakeEmployeeThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { PageSize = 2 });

                //Assert
                employeeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                employeeRepo.Should().ContainEquivalentOf(fakeEmployeeOne);
                employeeRepo.Should().ContainEquivalentOf(fakeEmployeeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ReturnExpectedPageNumberAndSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Ignore(e => e.Id).Generate();
            fakeEmployeeOne.Id = 1;
            var fakeEmployeeTwo = new FakeEmployee { }.Ignore(e => e.Id).Generate();
            fakeEmployeeTwo.Id = 2;
            var fakeEmployeeThree = new FakeEmployee { }.Ignore(e => e.Id).Generate();
            fakeEmployeeThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { PageSize = 1, PageNumber = 2 });

                //Assert
                employeeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                employeeRepo.Should().ContainEquivalentOf(fakeEmployeeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListEmployeeIdSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.Id = 2;

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.Id = 1;

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "Id" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeTwo, fakeEmployeeOne, fakeEmployeeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListEmployeeIdSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.Id = 2;

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.Id = 1;

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "-Id" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeThree, fakeEmployeeOne, fakeEmployeeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListFirstNameSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.FirstName = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.FirstName = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.FirstName = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "FirstName" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeTwo, fakeEmployeeOne, fakeEmployeeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListFirstNameSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.FirstName = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.FirstName = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.FirstName = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "-FirstName" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeThree, fakeEmployeeOne, fakeEmployeeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListLastNameSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.LastName = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.LastName = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.LastName = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "LastName" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeTwo, fakeEmployeeOne, fakeEmployeeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListLastNameSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.LastName = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.LastName = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.LastName = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "-LastName" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeThree, fakeEmployeeOne, fakeEmployeeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListDocIdSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.DocId = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.DocId = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.DocId = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "DocId" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeTwo, fakeEmployeeOne, fakeEmployeeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListDocIdSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.DocId = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.DocId = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.DocId = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "-DocId" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeThree, fakeEmployeeOne, fakeEmployeeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListPhoneNumberSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.PhoneNumber = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.PhoneNumber = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.PhoneNumber = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "PhoneNumber" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeTwo, fakeEmployeeOne, fakeEmployeeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListPhoneNumberSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.PhoneNumber = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.PhoneNumber = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.PhoneNumber = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "-PhoneNumber" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeThree, fakeEmployeeOne, fakeEmployeeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListEmailAddressSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.EmailAddress = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.EmailAddress = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.EmailAddress = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "EmailAddress" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeTwo, fakeEmployeeOne, fakeEmployeeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_ListEmailAddressSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.EmailAddress = "bravo";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.EmailAddress = "alpha";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.EmailAddress = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { SortOrder = "-EmailAddress" });

                //Assert
                employeeRepo.Should()
                    .ContainInOrder(fakeEmployeeThree, fakeEmployeeOne, fakeEmployeeTwo);

                context.Database.EnsureDeleted();
            }
        }


        [Fact]
        public async void GetEmployeesAsync_FilterEmployeeIdListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.Id = 1;

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.Id = 2;

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.Id = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { Filters = $"Id == {fakeEmployeeTwo.Id}" });

                //Assert
                employeeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_FilterFirstNameListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.FirstName = "alpha";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.FirstName = "bravo";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.FirstName = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { Filters = $"FirstName == {fakeEmployeeTwo.FirstName}" });

                //Assert
                employeeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_FilterLastNameListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.LastName = "alpha";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.LastName = "bravo";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.LastName = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { Filters = $"LastName == {fakeEmployeeTwo.LastName}" });

                //Assert
                employeeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_FilterDocIdListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.DocId = "alpha";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.DocId = "bravo";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.DocId = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { Filters = $"DocId == {fakeEmployeeTwo.DocId}" });

                //Assert
                employeeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_FilterPhoneNumerListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.PhoneNumber = "alpha";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.PhoneNumber = "bravo";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.PhoneNumber = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { Filters = $"PhoneNumber == {fakeEmployeeTwo.PhoneNumber}" });

                //Assert
                employeeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_FilterEmployeeDepartmentIdListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.EmployeeDepartmentId = 1;

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.EmployeeDepartmentId = 2;

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.EmployeeDepartmentId = 3;

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { Filters = $"EmployeeDepartmentId == {fakeEmployeeTwo.EmployeeDepartmentId}" });

                //Assert
                employeeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetEmployeesAsync_FilterEmailAddressListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<VisitPopDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmployeeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeEmployeeOne = new FakeEmployee { }.Generate();
            fakeEmployeeOne.EmailAddress = "alpha";

            var fakeEmployeeTwo = new FakeEmployee { }.Generate();
            fakeEmployeeTwo.EmailAddress = "bravo";

            var fakeEmployeeThree = new FakeEmployee { }.Generate();
            fakeEmployeeThree.EmailAddress = "charlie";

            //Act
            using (var context = new VisitPopDbContext(dbOptions))
            {
                context.Employees.AddRange(fakeEmployeeOne, fakeEmployeeTwo, fakeEmployeeThree);
                context.SaveChanges();

                var service = new EmployeeRepository(context, new SieveProcessor(sieveOptions));

                var employeeRepo = await service.GetEmployeesAsync(new EmployeeParametersDto { Filters = $"EmailAddress == {fakeEmployeeTwo.EmailAddress}" });

                //Assert
                employeeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }
    }
}

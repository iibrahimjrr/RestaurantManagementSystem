using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.Features.Staff.Commands;
using RestaurantManagementSystem.Application.Mapping;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;
using Xunit;

namespace RestaurantManagementSystem.Tests
{
    public class CreateStaffCommandHandlerTests
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateStaffCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_CreateStaff")
                .Options;
            _context = new AppDbContext(options);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldCreateStaff()
        {
            var handler = new CreateStaffCommandHandler(_context, _mapper);

            var command = new CreateStaffCommand
            {
                FullName = "John Doe",
                Position = "Chef",
                DateOfBirth = new DateTime(1985, 5, 20),
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                HireDate = DateTime.UtcNow,
                Salary = 50000m,
                Shift = "Morning",
                IsActive = true
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.FullName, result.FullName);
            Assert.Equal(command.Position, result.Position);
            Assert.Equal(command.Email, result.Email);
            Assert.Equal(command.PhoneNumber, result.PhoneNumber);
            Assert.Equal(command.Salary, result.Salary);
            Assert.Equal(command.Shift, result.Shift);
            Assert.True(result.IsActive);
        }
    }
}

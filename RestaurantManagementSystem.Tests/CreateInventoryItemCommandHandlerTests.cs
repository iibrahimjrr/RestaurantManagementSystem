using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Application.Features.Inventory.Commands;
using RestaurantManagementSystem.Application.Mapping;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;
using Xunit;

namespace RestaurantManagementSystem.Tests
{
    public class CreateInventoryItemCommandHandlerTests
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateInventoryItemCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_CreateInventory")
                .Options;
            _context = new AppDbContext(options);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldCreateInventoryItem()
        {
            var handler = new CreateInventoryItemCommandHandler(_context, _mapper);

            var command = new CreateInventoryItemCommand
            {
                ItemName = "Test Item",
                Description = "Test Description",
                Quantity = 10,
                Supplier = "Test Supplier",
                UnitPrice = 5.99m,
                ReorderLevel = 2
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.ItemName, result.ItemName);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.Quantity, result.Quantity);
            Assert.Equal(command.Supplier, result.Supplier);
            Assert.Equal(command.UnitPrice, result.UnitPrice);
            Assert.Equal(command.ReorderLevel, result.ReorderLevel);
        }
    }
}

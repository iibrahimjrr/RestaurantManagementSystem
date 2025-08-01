using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.Features.Products.Commands;
using RestaurantManagementSystem.Application.Mapping;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;
using Xunit;

namespace RestaurantManagementSystem.Tests
{
    public class CreateProductCommandHandlerTests
    {
        private readonly IMapper _mapper;

        public CreateProductCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using var context = new AppDbContext(options);
            var handler = new CreateProductCommandHandler(context, _mapper);

            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.5m,
                StockQuantity = 5
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.Price, result.Price);
            Assert.Equal(command.StockQuantity, result.StockQuantity);

            var productInDb = await context.Products.FirstOrDefaultAsync(p => p.Id == result.Id);
            Assert.NotNull(productInDb);
        }
    }
}

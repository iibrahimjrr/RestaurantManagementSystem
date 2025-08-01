using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.Features.POS.Commands;
using RestaurantManagementSystem.Application.Mapping;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;
using Xunit;

namespace RestaurantManagementSystem.Tests
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_CreateOrder")
                .Options;
            _context = new AppDbContext(options);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldCreateOrder()
        {
            var handler = new CreateOrderCommandHandler(_context, _mapper);

            var command = new CreateOrderCommand
            {
                OrderDate = DateTime.UtcNow,
                TableNumber = 1,
                TotalAmount = 100.00m,
                PaymentStatus = "Paid",
                OrderStatus = "Completed",
                OrderItems = {
                    new Application.DTOs.OrderItemDto
                    {
                        ProductId = 1,
                        Quantity = 2,
                        UnitPrice = 50.00m
                    }
                }
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.TableNumber, result.TableNumber);
            Assert.Equal(command.TotalAmount, result.TotalAmount);
            Assert.Equal(command.PaymentStatus, result.PaymentStatus);
            Assert.Equal(command.OrderStatus, result.OrderStatus);
            Assert.Single(result.OrderItems);
        }
    }
}

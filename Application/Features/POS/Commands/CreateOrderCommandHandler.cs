using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;

namespace RestaurantManagementSystem.Application.Features.POS.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                OrderDate = request.OrderDate,
                TableNumber = request.TableNumber,
                TotalAmount = request.TotalAmount,
                PaymentStatus = request.PaymentStatus,
                OrderStatus = request.OrderStatus
            };

            foreach (var itemDto in request.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice
                };
                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<OrderDto>(order);
        }
    }
}

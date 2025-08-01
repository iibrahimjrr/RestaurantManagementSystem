using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;

namespace RestaurantManagementSystem.Application.Features.POS.Commands
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderDto?>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto?> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            entity.OrderDate = request.OrderDate;
            entity.TableNumber = request.TableNumber;
            entity.TotalAmount = request.TotalAmount;
            entity.PaymentStatus = request.PaymentStatus;
            entity.OrderStatus = request.OrderStatus;

            // Update order items
            // Remove deleted items
            var itemsToRemove = entity.OrderItems.Where(oi => !request.OrderItems.Any(ri => ri.Id == oi.Id)).ToList();
            _context.OrderItems.RemoveRange(itemsToRemove);

            // Update existing and add new items
            foreach (var itemDto in request.OrderItems)
            {
                var existingItem = entity.OrderItems.FirstOrDefault(oi => oi.Id == itemDto.Id);
                if (existingItem != null)
                {
                    existingItem.ProductId = itemDto.ProductId;
                    existingItem.Quantity = itemDto.Quantity;
                    existingItem.UnitPrice = itemDto.UnitPrice;
                }
                else
                {
                    var newItem = new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice
                    };
                    entity.OrderItems.Add(newItem);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<OrderDto>(entity);
        }
    }
}

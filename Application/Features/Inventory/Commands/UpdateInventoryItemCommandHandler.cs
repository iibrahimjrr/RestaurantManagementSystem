using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;

namespace RestaurantManagementSystem.Application.Features.Inventory.Commands
{
    public class UpdateInventoryItemCommandHandler : IRequestHandler<UpdateInventoryItemCommand, InventoryItemDto?>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UpdateInventoryItemCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InventoryItemDto?> Handle(UpdateInventoryItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.InventoryItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            entity.ItemName = request.ItemName;
            entity.Description = request.Description;
            entity.Quantity = request.Quantity;
            entity.Supplier = request.Supplier;
            entity.UnitPrice = request.UnitPrice;
            entity.ReorderLevel = request.ReorderLevel;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<InventoryItemDto>(entity);
        }
    }
}

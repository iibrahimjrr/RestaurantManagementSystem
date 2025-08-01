using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;

namespace RestaurantManagementSystem.Application.Features.Inventory.Commands
{
    public class CreateInventoryItemCommandHandler : IRequestHandler<CreateInventoryItemCommand, InventoryItemDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateInventoryItemCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InventoryItemDto> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new InventoryItem
            {
                ItemName = request.ItemName,
                Description = request.Description,
                Quantity = request.Quantity,
                Supplier = request.Supplier,
                UnitPrice = request.UnitPrice,
                ReorderLevel = request.ReorderLevel
            };

            _context.InventoryItems.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<InventoryItemDto>(entity);
        }
    }
}

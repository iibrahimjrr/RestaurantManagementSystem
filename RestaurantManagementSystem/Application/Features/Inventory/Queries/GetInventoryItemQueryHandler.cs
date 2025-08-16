using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Infrastructure.Persistence;

namespace RestaurantManagementSystem.Application.Features.Inventory.Queries
{
    public class GetInventoryItemQueryHandler : IRequestHandler<GetInventoryItemQuery, InventoryItemDto?>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetInventoryItemQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InventoryItemDto?> Handle(GetInventoryItemQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.InventoryItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<InventoryItemDto>(entity);
        }
    }
}

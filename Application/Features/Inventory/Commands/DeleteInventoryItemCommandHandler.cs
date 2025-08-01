using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Infrastructure.Persistence;

namespace RestaurantManagementSystem.Application.Features.Inventory.Commands
{
    public class DeleteInventoryItemCommandHandler : IRequestHandler<DeleteInventoryItemCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteInventoryItemCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteInventoryItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.InventoryItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return false;
            }

            _context.InventoryItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

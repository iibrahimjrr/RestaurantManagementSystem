using MediatR;

namespace RestaurantManagementSystem.Application.Features.Inventory.Commands
{
    public class DeleteInventoryItemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

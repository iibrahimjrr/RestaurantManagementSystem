using MediatR;
using RestaurantManagementSystem.Application.DTOs;

namespace RestaurantManagementSystem.Application.Features.Inventory.Queries
{
    public class GetInventoryItemQuery : IRequest<InventoryItemDto?>
    {
        public int Id { get; set; }
    }
}

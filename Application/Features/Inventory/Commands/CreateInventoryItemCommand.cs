using MediatR;
using RestaurantManagementSystem.Application.DTOs;

namespace RestaurantManagementSystem.Application.Features.Inventory.Commands
{
    public class CreateInventoryItemCommand : IRequest<InventoryItemDto>
    {
        public string ItemName { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int ReorderLevel { get; set; }
    }
}

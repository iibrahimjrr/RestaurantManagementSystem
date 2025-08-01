using MediatR;
using RestaurantManagementSystem.Application.DTOs;

namespace RestaurantManagementSystem.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}

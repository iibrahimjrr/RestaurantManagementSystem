using MediatR;

namespace RestaurantManagementSystem.Application.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

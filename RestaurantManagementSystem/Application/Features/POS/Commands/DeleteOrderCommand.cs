using MediatR;

namespace RestaurantManagementSystem.Application.Features.POS.Commands
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

using MediatR;

namespace RestaurantManagementSystem.Application.Features.Staff.Commands
{
    public class DeleteStaffCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

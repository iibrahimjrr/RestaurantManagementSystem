using MediatR;
using RestaurantManagementSystem.Application.DTOs;

namespace RestaurantManagementSystem.Application.Features.Staff.Queries
{
    public class GetStaffQuery : IRequest<StaffDto?>
    {
        public int Id { get; set; }
    }
}

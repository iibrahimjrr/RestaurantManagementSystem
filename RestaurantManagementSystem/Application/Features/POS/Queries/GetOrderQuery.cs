using MediatR;
using RestaurantManagementSystem.Application.DTOs;

namespace RestaurantManagementSystem.Application.Features.POS.Queries
{
    public class GetOrderQuery : IRequest<OrderDto?>
    {
        public int Id { get; set; }
    }
}

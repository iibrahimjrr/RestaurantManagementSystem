using System;
using System.Collections.Generic;
using MediatR;
using RestaurantManagementSystem.Application.DTOs;

namespace RestaurantManagementSystem.Application.Features.POS.Commands
{
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public DateTime OrderDate { get; set; }
        public int TableNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}

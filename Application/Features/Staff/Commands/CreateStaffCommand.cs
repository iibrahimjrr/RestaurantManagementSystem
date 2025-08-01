using System;
using MediatR;
using RestaurantManagementSystem.Application.DTOs;

namespace RestaurantManagementSystem.Application.Features.Staff.Commands
{
    public class CreateStaffCommand : IRequest<StaffDto>
    {
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string Shift { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}

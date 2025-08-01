using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;

namespace RestaurantManagementSystem.Application.Features.Staff.Commands
{
    public class UpdateStaffCommandHandler : IRequestHandler<UpdateStaffCommand, StaffDto?>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UpdateStaffCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StaffDto?> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Staffs.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            entity.FullName = request.FullName;
            entity.Position = request.Position;
            entity.DateOfBirth = request.DateOfBirth;
            entity.Email = request.Email;
            entity.PhoneNumber = request.PhoneNumber;
            entity.HireDate = request.HireDate;
            entity.Salary = request.Salary;
            entity.Shift = request.Shift;
            entity.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StaffDto>(entity);
        }
    }
}

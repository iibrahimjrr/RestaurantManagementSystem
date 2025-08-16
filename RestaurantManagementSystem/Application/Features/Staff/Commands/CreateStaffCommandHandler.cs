using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;


namespace RestaurantManagementSystem.Application.Features.Staff.Commands
{
    public class CreateStaffCommandHandler : IRequestHandler<CreateStaffCommand, StaffDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateStaffCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StaffDto> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            var staff = new RestaurantManagementSystem.Core.Entities.Staff
            {
                FullName = request.FullName,
                Position = request.Position,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                HireDate = request.HireDate,
                Salary = request.Salary,
                Shift = request.Shift,
                IsActive = request.IsActive
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StaffDto>(staff);
        }
    }
}

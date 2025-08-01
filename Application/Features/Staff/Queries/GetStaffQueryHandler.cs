using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Infrastructure.Persistence;

namespace RestaurantManagementSystem.Application.Features.Staff.Queries
{
    public class GetStaffQueryHandler : IRequestHandler<GetStaffQuery, StaffDto?>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetStaffQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StaffDto?> Handle(GetStaffQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Staffs.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<StaffDto>(entity);
        }
    }
}

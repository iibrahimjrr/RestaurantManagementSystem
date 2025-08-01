using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Core.Entities;
using RestaurantManagementSystem.Infrastructure.Persistence;

namespace RestaurantManagementSystem.Application.Features.Reservation.Commands
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ReservationDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateReservationCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = new Reservation
            {
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                ReservationDate = request.ReservationDate,
                NumberOfGuests = request.NumberOfGuests,
                TableNumber = request.TableNumber,
                Status = request.Status,
                Notes = request.Notes
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReservationDto>(reservation);
        }
    }
}

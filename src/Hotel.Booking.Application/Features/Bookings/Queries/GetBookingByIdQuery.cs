using MediatR;
using Hotel.Booking.Domain.Common;
using Hotel.Booking.Domain.Entities;

namespace Hotel.Booking.Application.Features.Bookings.Queries
{
    public sealed record GetBookingByIdQuery(int Id) : IRequest<Result<Domain.Entities.Bookings>>;
}

using MediatR;
using Hotel.Booking.Domain.Common;
using Hotel.Booking.Domain.Entities;

namespace Hotel.Booking.Application.Features.Bookings.Commands
{
    public sealed record CreateBookingCommand(int HotelId, int RoomTypeId, int GuestId, DateTime CheckIn, DateTime CheckOut, int Quantity, string? IdempotencyKey = null) : IRequest<Result<int>>;
}

using MediatR;
using Hotel.Booking.Domain.Common;
using System.Collections.Generic;

namespace Hotel.Booking.Application.Features.Bookings.Queries
{
    public sealed record AvailabilityQuery(int HotelId, DateTime From, DateTime To, int Guests) : IRequest<Result<List<AvailabilityDto>>>;

    public sealed record AvailabilityDto(int RoomTypeId, string Name, int Capacity, int Available, decimal PricePerNight);
}

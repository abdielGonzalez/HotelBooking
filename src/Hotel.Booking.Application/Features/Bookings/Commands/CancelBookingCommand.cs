using MediatR;
using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Application.Features.Bookings.Commands;

public sealed record CancelBookingCommand(int Id) : IRequest<Result<int>>;

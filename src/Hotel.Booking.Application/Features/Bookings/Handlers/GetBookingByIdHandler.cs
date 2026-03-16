using MediatR;
using Hotel.Booking.Domain.Common;
using Hotel.Booking.Application.Interfaces;
using Hotel.Booking.Application.Features.Bookings.Queries;

namespace Hotel.Booking.Application.Features.Bookings.Handlers;

public class GetBookingByIdHandler : IRequestHandler<GetBookingByIdQuery, Result<Domain.Entities.Bookings>>
{
    private readonly IBookingRepository _repo;

    public GetBookingByIdHandler(IBookingRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Domain.Entities.Bookings>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (booking == null) return Result.Failure<Domain.Entities.Bookings>(Domain.Common.Error.NotFound("Booking.NotFound", "Booking not found"));
        return Result.Success(booking);
    }
}

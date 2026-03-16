using MediatR;
using Hotel.Booking.Domain.Common;
using Hotel.Booking.Application.Interfaces;
using Hotel.Booking.Application.Features.Bookings.Queries;

namespace Hotel.Booking.Application.Features.Bookings.Handlers;

public class AvailabilityHandler : IRequestHandler<AvailabilityQuery, Result<List<AvailabilityDto>>>
{
    private readonly IBookingRepository _repo;

    public AvailabilityHandler(IBookingRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<List<AvailabilityDto>>> Handle(AvailabilityQuery request, CancellationToken cancellationToken)
    {
        if (request.From >= request.To)
            return Result.Failure<List<AvailabilityDto>>(new Domain.Common.Error("Booking.InvalidRange", "check-in must be before check-out", Domain.Common.ErrorType.Failure));

        if ((request.To - request.From).TotalDays > 30)
            return Result.Failure<List<AvailabilityDto>>(new Domain.Common.Error("Booking.RangeTooLarge", "range exceeds maximum allowed nights", Domain.Common.ErrorType.Failure));

        var roomTypes = await _repo.GetActiveRoomTypesByHotelAsync(request.HotelId, cancellationToken);
        var results = new List<AvailabilityDto>();

        foreach (var rt in roomTypes)
        {
            if (rt.Capacity < request.Guests) continue;

            var inventories = await _repo.GetInventoriesAsync(rt.RoomTypeId, request.From, request.To, cancellationToken);
            if (inventories.Count != (request.To - request.From).Days)
                continue;

            var minAvailable = inventories.Min(i => i.Available);
            if (minAvailable <= 0) continue;

            var rate = await _repo.GetRatePlanForRoomTypeAsync(rt.RoomTypeId, cancellationToken);

            results.Add(new AvailabilityDto(rt.RoomTypeId, rt.Name, rt.Capacity, minAvailable, rate?.PricePerNight ?? 0));
        }

        return Result.Success(results);
    }
}

using MediatR;
using Hotel.Booking.Domain.Common;
using Hotel.Booking.Application.Interfaces;
using Hotel.Booking.Application.Features.Bookings.Queries;

namespace Hotel.Booking.Application.Features.Bookings.Handlers;

public class GetBookingsHandler : IRequestHandler<GetBookingsQuery, Result<PagedResult<Domain.Entities.Bookings>>>
{
    private readonly IBookingRepository _repo;

    public GetBookingsHandler(IBookingRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<PagedResult<Domain.Entities.Bookings>>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _repo.GetBookingsPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        var result = new PagedResult<Domain.Entities.Bookings>(items, request.PageNumber, request.PageSize, total, (int)Math.Ceiling(total / (double)request.PageSize));
        return Result.Success(result);
    }
}

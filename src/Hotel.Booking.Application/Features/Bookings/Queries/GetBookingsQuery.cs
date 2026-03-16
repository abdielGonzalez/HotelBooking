using MediatR;
using Hotel.Booking.Domain.Common;
using Hotel.Booking.Domain.Entities;
using System.Collections.Generic;

namespace Hotel.Booking.Application.Features.Bookings.Queries
{
    public sealed record GetBookingsQuery(int PageNumber, int PageSize) : IRequest<Result<PagedResult<Domain.Entities.Bookings>>>;

    public sealed record PagedResult<T>(List<T> Data, int PageNumber, int PageSize, int TotalRecords, int TotalPages);
}

using MediatR;
using Hotel.Booking.Domain.Common;
using Hotel.Booking.Application.Interfaces;
using Hotel.Booking.Application.Features.Bookings.Commands;

namespace Hotel.Booking.Application.Features.Bookings.Handlers;

public class ConfirmBookingHandler : IRequestHandler<ConfirmBookingCommand, Result<int>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBookingRepository _repo;

    public ConfirmBookingHandler(IUnitOfWork uow, IBookingRepository repo)
    {
        _uow = uow;
        _repo = repo;
    }

    public async Task<Result<int>> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var booking = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (booking == null) return Result.Failure<int>(Domain.Common.Error.NotFound("Booking.NotFound", "Booking not found"));

            booking.Confirm();
            await _repo.UpdateAsync(booking, cancellationToken);
            await _uow.CommitAsync(cancellationToken);
            return Result.Success(booking.BookingId);
        }
        catch (Exception ex)
        {
            await _uow.RollbackAsync(cancellationToken);
            return Result.Failure<int>(Domain.Common.Error.Problem("Booking.ConfirmFailed", ex.Message));
        }
    }
}

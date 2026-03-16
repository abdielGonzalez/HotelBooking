using MediatR;
using Hotel.Booking.Domain.Common;
using Hotel.Booking.Application.Interfaces;
using Hotel.Booking.Application.Features.Bookings.Commands;
using Hotel.Booking.Domain.Entities;

namespace Hotel.Booking.Application.Features.Bookings.Handlers
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, Result<int>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBookingRepository _repo;

        public CreateBookingHandler(IUnitOfWork uow, IBookingRepository repo)
        {
            _uow = uow;
            _repo = repo;
        }

        public async Task<Result<int>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // validate
            if (request.CheckIn >= request.CheckOut)
                return Result.Failure<int>(Domain.Common.Error.Problem("Booking.InvalidRange", "check-in must be before check-out"));

            if ((request.CheckOut - request.CheckIn).TotalDays > 30)
                return Result.Failure<int>(Domain.Common.Error.Problem("Booking.RangeTooLarge", "range exceeds maximum allowed nights"));

            var idempotencyKey = request.IdempotencyKey ?? string.Empty; // caller provides header via controller
            try
            {
                // Acquire app lock to avoid concurrency issues
                await _repo.AcquireAppLockAsync($"booking_{request.RoomTypeId}", 10000, cancellationToken);

                // Check idempotency
                var existing = string.IsNullOrEmpty(idempotencyKey) ? null : await _repo.GetIdempotencyRecordAsync(idempotencyKey, cancellationToken);
                if (existing != null)
                {
                    return Result.Success(existing.Id); // return original booking id if stored
                }

                // Create booking entity
                var booking = new Domain.Entities.Bookings(request.HotelId, request.RoomTypeId, request.GuestId, request.CheckIn, request.CheckOut, request.Quantity, 0);
                await _repo.CreateBookingAsync(booking, cancellationToken);

                // Decrement inventory
                await _repo.DecreaseInventoryAsync(request.RoomTypeId, request.CheckIn, request.CheckOut, request.Quantity, cancellationToken);
            // After success, persist idempotency response containing booking id and body
            if (!string.IsNullOrEmpty(idempotencyKey))
            {
                var body = System.Text.Json.JsonSerializer.Serialize(new { id = booking.BookingId });
                var record = new IdempotencyRecord(idempotencyKey, null, 201, body, DateTime.UtcNow.AddHours(1));
                await _repo.AddIdempotencyRecordAsync(record, cancellationToken);
            }

                await _uow.SaveChangesAsync(cancellationToken);
                await _uow.CommitAsync(cancellationToken);

                return Result.Success(booking.BookingId);
            }
            catch (InvalidOperationException iox)
            {
                await _uow.RollbackAsync(cancellationToken);
                return Result.Failure<int>(Domain.Common.Error.Conflict("Booking.InsufficientInventory", iox.Message));
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync(cancellationToken);
                return Result.Failure<int>(Domain.Common.Error.Problem("Booking.CreateFailed", ex.Message));
            }
        }
    }
}

using Hotel.Booking.Domain.Entities;

namespace Hotel.Booking.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<RoomType>> GetActiveRoomTypesByHotelAsync(int hotelId, CancellationToken cancellationToken = default);
        Task<List<RoomInventory>> GetInventoriesAsync(int roomTypeId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
        Task<RatePlan?> GetRatePlanForRoomTypeAsync(int roomTypeId, CancellationToken cancellationToken = default);
        Task<(List<Domain.Entities.Bookings> Items, int Total)> GetBookingsPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<Domain.Entities.Bookings?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Domain.Entities.Bookings booking, CancellationToken cancellationToken = default);
        Task AcquireAppLockAsync(string key, int timeoutMs = 10000, CancellationToken cancellationToken = default);
        Task<IdempotencyRecord?> GetIdempotencyRecordAsync(string key, CancellationToken cancellationToken = default);
        Task AddIdempotencyRecordAsync(IdempotencyRecord record, CancellationToken cancellationToken = default);
        Task CreateBookingAsync(Domain.Entities.Bookings booking, CancellationToken cancellationToken = default);
        Task DecreaseInventoryAsync(int roomTypeId, DateTime from, DateTime to, int quantity, CancellationToken cancellationToken = default);
    }
}
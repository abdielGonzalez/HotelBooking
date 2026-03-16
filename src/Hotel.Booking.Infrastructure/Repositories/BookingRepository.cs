using Hotel.Booking.Application.Interfaces;
using Hotel.Booking.Domain.Entities;
using Hotel.Booking.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Booking.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly CardsBdContext _db;

        public BookingRepository(CardsBdContext db)
        {
            _db = db;
        }

        public async Task<List<RoomType>> GetActiveRoomTypesByHotelAsync(int hotelId, CancellationToken cancellationToken = default)
        {
            return await _db.RoomTypes!.Where(rt => rt.HotelId == hotelId && rt.IsActive).ToListAsync(cancellationToken);
        }

        public async Task<List<RoomInventory>> GetInventoriesAsync(int roomTypeId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            return await _db.RoomInventories!
                .Where(i => i.RoomTypeId == roomTypeId && i.Date >= from.Date && i.Date < to.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<RatePlan?> GetRatePlanForRoomTypeAsync(int roomTypeId, CancellationToken cancellationToken = default)
        {
            return await _db.RatePlans!.FirstOrDefaultAsync(rp => rp.RoomTypeId == roomTypeId, cancellationToken);
        }

        public async Task<(List<Domain.Entities.Bookings> Items, int Total)> GetBookingsPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _db.Bookings!.AsQueryable();
            var total = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(b => b.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return (items, total);
        }

        public async Task<Domain.Entities.Bookings?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.Bookings!.FindAsync(new object[] { id }, cancellationToken);
        }

        public Task UpdateAsync(Domain.Entities.Bookings booking, CancellationToken cancellationToken = default)
        {
            _db.Bookings!.Update(booking);
            return Task.CompletedTask;
        }

        public async Task AcquireAppLockAsync(string key, int timeoutMs = 10000, CancellationToken cancellationToken = default)
        {
            var sql = "sp_getapplock @Resource=@p0, @LockMode='Exclusive', @LockTimeout=@p1;";
            await _db.Database.ExecuteSqlRawAsync(sql, new object[] { key, timeoutMs }, cancellationToken);
        }

        public async Task<IdempotencyRecord?> GetIdempotencyRecordAsync(string key, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _db.IdempotencyRecords!.FirstOrDefaultAsync(r => r.Key == key && r.ExpiresAt > now, cancellationToken);
        }

        public Task AddIdempotencyRecordAsync(IdempotencyRecord record, CancellationToken cancellationToken = default)
        {
            _db.IdempotencyRecords!.Add(record);
            return Task.CompletedTask;
        }

        public Task CreateBookingAsync(Domain.Entities.Bookings booking, CancellationToken cancellationToken = default)
        {
            _db.Bookings!.Add(booking);
            return Task.CompletedTask;
        }

        public async Task DecreaseInventoryAsync(int roomTypeId, DateTime from, DateTime to, int quantity, CancellationToken cancellationToken = default)
        {
            var inventories = await _db.RoomInventories!
                .Where(i => i.RoomTypeId == roomTypeId && i.Date >= from.Date && i.Date < to.Date)
                .ToListAsync(cancellationToken);

            foreach (var inv in inventories)
            {
                // ensure no negative inventory
                var newVal = inv.Available - quantity;
                if (newVal < 0) throw new InvalidOperationException("Insufficient inventory");
                inv.GetType().GetProperty("Available")!.SetValue(inv, newVal);
            }
        }
    }
}
using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Domain.Entities
{
    public class RoomType : Entity
    {
        protected RoomType() { }

        public RoomType(int hotelId, string name, int capacity)
        {
            HotelId = hotelId;
            Name = name;
            Capacity = capacity;
            IsActive = true;
            Created = DateTime.UtcNow;
        }

        public int RoomTypeId { get; private set; }

        public int HotelId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public int Capacity { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime Created { get; private set; }

        public DateTime? Modified { get; private set; }

        public Hotels? Hotel { get; private set; }

        public List<RoomInventory> Inventories { get; private set; } = new();

        public List<RatePlan> RatePlans { get; private set; } = new();
    }
}
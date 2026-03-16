using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Domain.Entities
{
    public class Hotels : Entity
    {
        protected Hotels() { }

        public Hotels(string name, string address)
        {
            Name = name;
            Address = address;
            IsActive = true;
            Created = DateTime.UtcNow;
        }

        public int HotelId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Address { get; private set; } = string.Empty;

        public bool IsActive { get; private set; }

        public DateTime Created { get; private set; }

        public DateTime? Modified { get; private set; }

        public List<RoomType> RoomTypes { get; private set; } = new();
    }
}
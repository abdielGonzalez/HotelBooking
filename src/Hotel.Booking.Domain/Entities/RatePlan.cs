using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Domain.Entities
{
    public class RatePlan : Entity
    {
        protected RatePlan() { }

        public RatePlan(int roomTypeId, string name, decimal pricePerNight)
        {
            RoomTypeId = roomTypeId;
            Name = name;
            PricePerNight = pricePerNight;
            Created = DateTime.Now;
        }

        public int RatePlanId { get; private set; }

        public int RoomTypeId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public decimal PricePerNight { get; private set; }

        public DateTime Created { get; private set; }

        public DateTime? Modified { get; private set; }

        public RoomType? RoomType { get; private set; }
    }
}
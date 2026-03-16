using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Domain.Entities
{
    public class RoomInventory : Entity
    {
        protected RoomInventory() { }

        public RoomInventory(int roomTypeId, DateTime date, int available)
        {
            RoomTypeId = roomTypeId;
            Date = date.Date;
            Available = available;
            Created = DateTime.UtcNow;
        }

        public int RoomInventoryId { get; private set; }

        public int RoomTypeId { get; private set; }

        public DateTime Date { get; private set; }

        public int Available { get; private set; }

        public DateTime Created { get; private set; }

        public DateTime? Modified { get; private set; }

        public RoomType? RoomType { get; private set; }
    }
}
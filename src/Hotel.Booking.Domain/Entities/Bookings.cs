using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Domain.Entities
{
    public class Bookings : Entity
    {
        protected Bookings() { }

        public Bookings(int hotelId, int roomTypeId, int guestId, DateTime checkIn, DateTime checkOut, int quantity, decimal totalAmount)
        {
            HotelId = hotelId;
            RoomTypeId = roomTypeId;
            GuestId = guestId;
            CheckIn = checkIn.Date;
            CheckOut = checkOut.Date;
            Quantity = quantity;
            TotalAmount = totalAmount;
            Status = BookingStatus.Pending;
            Created = DateTime.UtcNow;
        }

        public int BookingId { get; private set; }

        public int HotelId { get; private set; }

        public int RoomTypeId { get; private set; }

        public int GuestId { get; private set; }

        public DateTime CheckIn { get; private set; }

        public DateTime CheckOut { get; private set; }

        public int Quantity { get; private set; }

        public decimal TotalAmount { get; private set; }

        public BookingStatus Status { get; private set; }

        public DateTime Created { get; private set; }

        public DateTime? Modified { get; private set; }
        
        public void Confirm()
        {
            if (Status == BookingStatus.Confirmed)
                throw new InvalidOperationException("Booking already confirmed.");

            if (Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Cannot confirm a cancelled booking.");

            Status = BookingStatus.Confirmed;
            Modified = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Booking already cancelled.");

            Status = BookingStatus.Cancelled;
            Modified = DateTime.UtcNow;
        }
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}
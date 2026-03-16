using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Domain.Entities
{
    public class Payment : Entity
    {
        protected Payment() { }

        public Payment(int bookingId, decimal amount, string method)
        {
            BookingId = bookingId;
            Amount = amount;
            Method = method;
            Created = DateTime.UtcNow;
        }

        public int PaymentId { get; private set; }

        public int BookingId { get; private set; }

        public decimal Amount { get; private set; }

        public string Method { get; private set; } = string.Empty;

        public DateTime Created { get; private set; }
    }
}
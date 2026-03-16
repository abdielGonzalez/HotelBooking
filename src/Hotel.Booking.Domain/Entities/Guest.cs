using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Domain.Entities
{
    public class Guest : Entity
    {
        protected Guest() { }

        public Guest(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Created = DateTime.UtcNow;
        }

        public int GuestId { get; private set; }

        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public DateTime Created { get; private set; }

        public DateTime? Modified { get; private set; }
    }
}
using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Domain.Entities
{
    public class IdempotencyRecord : Entity
    {
        protected IdempotencyRecord() { }

        public IdempotencyRecord(string key, string? requestHash, int responseStatus, string responseBody, DateTime expiresAt)
        {
            Key = key;
            RequestHash = requestHash;
            ResponseStatus = responseStatus;
            ResponseBody = responseBody;
            ExpiresAt = expiresAt;
            Created = DateTime.UtcNow;
        }

        public int Id { get; private set; }

        public string Key { get; private set; } = string.Empty;

        public string? RequestHash { get; private set; }

        public int ResponseStatus { get; private set; }

        public string ResponseBody { get; private set; } = string.Empty;

        public DateTime ExpiresAt { get; private set; }

        public DateTime Created { get; private set; }
    }
}
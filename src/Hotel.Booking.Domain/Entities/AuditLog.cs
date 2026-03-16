using Hotel.Booking.Domain.Common;

namespace Hotel.Booking.Domain.Entities
{
    public class AuditLog : Entity
    {
        protected AuditLog() { }

        public AuditLog(string correlationId, string message, string? data = null)
        {
            CorrelationId = correlationId;
            Message = message;
            Data = data;
            Created = DateTime.UtcNow;
        }

        public int AuditLogId { get; private set; }

        public string CorrelationId { get; private set; } = string.Empty;

        public string Message { get; private set; } = string.Empty;

        public string? Data { get; private set; }

        public DateTime Created { get; private set; }
    }
}
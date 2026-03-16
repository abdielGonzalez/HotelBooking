using System.Data.Common;

namespace Hotel.Booking.Application.Common.Interfaces
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> OpenConnectionAsync();
    }
}

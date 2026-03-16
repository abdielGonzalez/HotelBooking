using Hotel.Booking.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Hotel.Booking.Infrastructure.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DbConnection> OpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hotel.Booking.Infrastructure.Database.Converters
{
    public sealed class DateOnlyConverter
    : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter()
            : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        {
        }
    }
}

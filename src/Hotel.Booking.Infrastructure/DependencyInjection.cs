using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hotel.Booking.Infrastructure.Database;
//using Hotel.Booking.Application.Interfaces.Persistence;
using Hotel.Booking.Infrastructure.Repositories;
using Hotel.Booking.Application.Interfaces;
using Hotel.Booking.Application.Common.Validators.Catalogs;
//using Hotel.Booking.Infrastructure.Catalogs;

namespace Hotel.Booking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CardsBdContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("HotelsDb")));
            services.AddScoped<IBookingRepository, BookingRepository>(); // Updated booking repository
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Hotel.Booking.Application.AssemblyReference).Assembly));
            //services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
}

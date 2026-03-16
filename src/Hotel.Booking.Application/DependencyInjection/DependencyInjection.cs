using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Booking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(AssemblyReference).Assembly));

        return services;
    }
}

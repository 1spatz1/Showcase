using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Showcase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // services.AddScoped(
        //     typeof(IPipelineBehavior<,>));
        
        return services;
    }
}
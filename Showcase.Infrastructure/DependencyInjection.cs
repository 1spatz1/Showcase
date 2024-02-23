using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Showcase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // services.AddScoped(
        //     typeof(IPipelineBehavior<,>));
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("/logs/log-infra.txt", rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Warning)
            .WriteTo.File("/logs/log-infra-verbose.txt", rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Verbose)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .CreateLogger();
        
        return services;
    }
}
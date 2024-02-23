using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Showcase.Application.Common.Behaviors;
using Showcase.Application.Common.Mapping;

namespace Showcase.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMapping();
        
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("/logs/log-app.txt", rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Warning)
            .WriteTo.File("/logs/log-app-verbose.txt", rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Verbose)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .CreateLogger();
        
        return services;
    }
}
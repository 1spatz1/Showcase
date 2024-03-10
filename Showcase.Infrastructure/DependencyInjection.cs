using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;
using Showcase.Infrastructure.Persistence;
using Showcase.Infrastructure.Services;
using Showcase.Utilities;

namespace Showcase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Add services
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        
        // Identity
        services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 8;
            })
            .AddRoles<ApplicationRole>()
            .AddRoleManager<RoleManager<ApplicationRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
        services.AddScoped<RoleManager<ApplicationRole>, RoleManager<ApplicationRole>>();
        
        // Logging
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
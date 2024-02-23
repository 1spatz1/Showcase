using Showcase.Api;
using Microsoft.AspNetCore.Cors;
using Serilog;
using Serilog.Events;
using Showcase.Application;
using Showcase.Infrastructure;

Directory.CreateDirectory("/logs");
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("/logs/log.txt", rollingInterval: RollingInterval.Month,
        restrictedToMinimumLevel: LogEventLevel.Warning)
    .WriteTo.File("/logs/log-verbose.txt", rollingInterval: RollingInterval.Month,
        restrictedToMinimumLevel: LogEventLevel.Verbose)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .CreateLogger();

DotNetEnv.Env.TraversePath().Load();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog();
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure()
        .AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
}

WebApplication app = builder.Build();
{
    app.UseSerilogRequestLogging();
    using (IServiceScope scope = app.Services.CreateScope())
    {
        IServiceProvider services = scope.ServiceProvider;
    }
    
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors("AllowAllOrigins");
    app.UseAuthorization();
    
    app.MapControllers();
    app.Run();
}
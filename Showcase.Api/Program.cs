using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Showcase.Api;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Showcase.Application;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;
using Showcase.Infrastructure;
using Showcase.Utilities;

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
        })
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = EnvironmentReader.Jwt.Issuer,
                ValidAudience = EnvironmentReader.Jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentReader.Jwt.SigningKey))
            };
        });
    
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
        options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
    });
}

WebApplication app = builder.Build();
{
    app.UseSerilogRequestLogging();
    using (IServiceScope scope = app.Services.CreateScope())
    {
        IServiceProvider services = scope.ServiceProvider;
        IApplicationDbContext context = services.GetRequiredService<IApplicationDbContext>();
        UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        RoleManager<ApplicationRole> roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        
        await context.Database.MigrateAsync();
        await DbSeeder.SeedDbAsync(context, userManager, roleManager);
    }
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAllOrigins");
    app.UseAuthorization();
    
    app.MapControllers();
    app.Run();
}
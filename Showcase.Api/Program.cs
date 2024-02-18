using Showcase.Api;
using Microsoft.AspNetCore.Cors;
using Showcase.Application;
using Showcase.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
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
using Microsoft.EntityFrameworkCore;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;
using Showcase.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Showcase.Api;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext Context { get; private set; }
    private UserManager<ApplicationUser> _userManager;
    private RoleManager<ApplicationRole> _roleManager;

    public DatabaseFixture()
{
    var services = new ServiceCollection();

    // Add logging services
    services.AddLogging();

    // Add Identity services
    services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // Add ApplicationDbContext using a SQL Server database for testing
    services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=TestDb_{Guid.NewGuid()};Trusted_Connection=True;");
    });

    var serviceProvider = services.BuildServiceProvider();

    // Get the UserManager and RoleManager from the service provider
    _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    // Get the ApplicationDbContext from the service provider and assign it to Context
    Context = serviceProvider.GetRequiredService<ApplicationDbContext>();

    // Migrate and seed the new database
    Context.Database.Migrate();
    DbSeeder.SeedDbAsync(Context, _userManager, _roleManager).Wait();
}

public void Dispose()
{
    // Delete the database after the test is done
    Context.Database.EnsureDeleted();

    Context.Dispose();
}

public void CreateNewContext()
{
    // Dispose of the old context and delete the old database
    Context.Database.EnsureDeleted();
    Context.Dispose();

    // Create a new context with a unique SQL Server database name
    var services = new ServiceCollection();

    // Add logging services
    services.AddLogging();

    // Add Identity services
    services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=TestDb_{Guid.NewGuid()};Trusted_Connection=True;");
    });

    var serviceProvider = services.BuildServiceProvider();

    // Get the UserManager and RoleManager from the service provider
    _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    Context = serviceProvider.GetRequiredService<ApplicationDbContext>();

    // Migrate and seed the new database
    Context.Database.Migrate();
    DbSeeder.SeedDbAsync(Context, _userManager, _roleManager).Wait();
}
}
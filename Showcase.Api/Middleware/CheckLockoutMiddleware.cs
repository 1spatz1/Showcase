using Showcase.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Showcase.Api.Middleware;

public class CheckLockoutMiddleware
{
    private readonly RequestDelegate _next;

    public CheckLockoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.GetUserAsync(context.User);
        if (user != null && await userManager.IsLockedOutAsync(user))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("User is locked out");
        }
        else
        {
            await _next(context);
        }
    }
}
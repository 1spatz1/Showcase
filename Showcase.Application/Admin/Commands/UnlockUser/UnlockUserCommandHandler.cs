using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Authentication.Commands.LockUser;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;

namespace Showcase.Application.Authentication.Commands.UnlockUser;

public class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand, ErrorOr<UnlockUserResponse>>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<UnlockUserCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    public UnlockUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<UnlockUserCommandHandler> logger, IDateTimeProvider dateTimeProvider, IApplicationDbContext context)
    {
        _dateTimeProvider = dateTimeProvider;
        _userManager = userManager;
        _logger = logger;
        _context = context; 
    }

    public async Task<ErrorOr<UnlockUserResponse>> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefault(u => u.Email == request.Email);
        
        if (user is null)
            return Errors.Authentication.InvalidCredentials;
        
        var roleNames = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        if(roleNames.Contains(IdentityNames.Roles.Administrator))
            return Errors.Admin.InvalidAction;

        var unlockUserResult = await UnlockUser(user);
        if (unlockUserResult.IsError)
            return Errors.UnexpectedError;

        var setLockoutEndDateResult = await SetLockoutEndDate(user);
        if (setLockoutEndDateResult.IsError)
            return Errors.UnexpectedError;

        return new UnlockUserResponse(true);
    }

    private async Task<ErrorOr<Unit>> UnlockUser(ApplicationUser user)
    {
        var result = await _userManager.SetLockoutEnabledAsync(user, true);
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to unlock user with email {Email} in database: {Message}", user.Email, result.Errors);
            return Errors.UnexpectedError;
        }

        return Unit.Value;
    }

    private async Task<ErrorOr<Unit>> SetLockoutEndDate(ApplicationUser user)
    {
        var result = await _userManager.SetLockoutEndDateAsync(user, _dateTimeProvider.UtcNow.Subtract(TimeSpan.FromMinutes(1)));
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to set lockout end date for user with email {Email} in database: {Message}", user.Email, result.Errors);
            return Errors.UnexpectedError;
        }

        return Unit.Value;
    }
}
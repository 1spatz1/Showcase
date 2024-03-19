using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Showcase.Application.Authentication.Commands.LockUser;
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

    public UnlockUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<UnlockUserCommandHandler> logger, IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ErrorOr<UnlockUserResponse>> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Errors.Authentication.InvalidCredentials;
        
        if(user.UserRoles.Any(role => role.Role.Name == IdentityNames.Roles.Administrator))
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
        Console.WriteLine(_dateTimeProvider.UtcNow.Subtract(TimeSpan.FromMinutes(1)));
        if (!result.Succeeded)
        {
            Console.WriteLine(result.Errors.ToString());
            _logger.LogError("Failed to set lockout end date for user with email {Email} in database: {Message}", user.Email, result.Errors);
            return Errors.UnexpectedError;
        }

        return Unit.Value;
    }
}
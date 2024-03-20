using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;

namespace Showcase.Application.Authentication.Commands.LockUser;

public class LockUserCommandHandler : IRequestHandler<LockUserCommand, ErrorOr<LockUserResponse>>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<LockUserCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    public LockUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<LockUserCommandHandler> logger, IDateTimeProvider dateTimeProvider, IApplicationDbContext context)
    {
        _dateTimeProvider = dateTimeProvider;
        _userManager = userManager;
        _logger = logger;
        _context = context; 
    }

    public async Task<ErrorOr<LockUserResponse>> Handle(LockUserCommand request, CancellationToken cancellationToken)
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

        var lockUserResult = await LockUser(user);
        if (lockUserResult.IsError)
            return Errors.UnexpectedError;

        var setLockoutEndDateResult = await SetLockoutEndDate(user, request.DurationDays);
        if (setLockoutEndDateResult.IsError)
            return Errors.UnexpectedError;

        return new LockUserResponse(true);
    }

    private async Task<ErrorOr<Unit>> LockUser(ApplicationUser user)
    {
        var result = await _userManager.SetLockoutEnabledAsync(user, true);
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to lock user with email {Email} in database: {Message}", user.Email, result.Errors);
            return Errors.UnexpectedError;
        }

        return Unit.Value;
    }

    private async Task<ErrorOr<Unit>> SetLockoutEndDate(ApplicationUser user, int durationDays)
    {
        var result = await _userManager.SetLockoutEndDateAsync(user, _dateTimeProvider.UtcNow.AddDays(durationDays));
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to set lockout end date for user with email {Email} in database: {Message}", user.Email, result.Errors);
            return Errors.UnexpectedError;
        }

        return Unit.Value;
    }
}
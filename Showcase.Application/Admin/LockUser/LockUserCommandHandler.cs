﻿using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Authentication.Commands.LockUser;

public class LockUserCommandHandler : IRequestHandler<LockUserCommand, ErrorOr<LockUserResponse>>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<LockUserCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public LockUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<LockUserCommandHandler> logger,
        IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ErrorOr<LockUserResponse>> Handle(LockUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Errors.Authentication.InvalidCredentials;

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
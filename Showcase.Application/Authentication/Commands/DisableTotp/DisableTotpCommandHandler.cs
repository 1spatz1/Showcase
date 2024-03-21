﻿using System.Security.Cryptography;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Showcase.Application.Authentication.Commands.ConfigureTotp;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Authentication.Commands.DisableTotp;

public class DisableTotpCommandHandler : IRequestHandler<DisableTotpCommand, ErrorOr<DisableTotpResponse>>
{
    private readonly ILogger<DisableTotpCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    
    public DisableTotpCommandHandler(UserManager<ApplicationUser> userManager, ILogger<DisableTotpCommandHandler> logger, IApplicationDbContext context)
    {
        _userManager = userManager;
        _logger = logger;
        _context = context; 
    }
    
    public async Task<ErrorOr<DisableTotpResponse>> Handle(DisableTotpCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _context.Users.FindAsync(request.UserId);
        if (user is null)
            return Errors.Authentication.InvalidCredentials;
        
        var disable2FaResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
        if (!disable2FaResult.Succeeded)
        {
            _logger.LogError("Failed to disable 2FA for user with email {Email} in database: {Message}", user.Email, disable2FaResult.Errors);
            return Errors.UnexpectedError;
        }
        
        var userTotpSecret = await _context.UserTotpSecrets.FindAsync(request.UserId);
        if (userTotpSecret is not null)
        {
            _context.UserTotpSecrets.Remove(userTotpSecret);
            if(await SaveToDb(userTotpSecret, cancellationToken) == true)
                return new DisableTotpResponse(true);
        }
        return new DisableTotpResponse(true);
    }
    
    private async Task<ErrorOr<Boolean>> SaveToDb(UserTotpSecret userTotpSecret , CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete userTotpSecret in database: {Message}", ex.Message);
            return false;
        }
    }
}
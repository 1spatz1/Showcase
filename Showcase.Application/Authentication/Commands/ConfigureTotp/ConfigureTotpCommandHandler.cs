using System.Security.Cryptography;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Showcase.Application.Authentication.Commands.LockUser;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Authentication.Commands.ConfigureTotp;

public class ConfigureTotpCommandHandler : IRequestHandler<ConfigureTotpCommand, ErrorOr<ConfigureTotpResponse>>
{
    private readonly ILogger<ConfigureTotpCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    
    public ConfigureTotpCommandHandler(UserManager<ApplicationUser> userManager, ILogger<ConfigureTotpCommandHandler> logger, IApplicationDbContext context)
    {
        _userManager = userManager;
        _logger = logger;
        _context = context; 
    }
    
    public async Task<ErrorOr<ConfigureTotpResponse>> Handle(ConfigureTotpCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _context.Users.FindAsync(request.UserId);
        if (user is null)
            return Errors.Authentication.InvalidCredentials;
        
        var totpSecret = await _context.UserTotpSecrets.FindAsync(request.UserId);
        if (totpSecret is not null)
            return Errors.Authentication.TotpAlreadyConfigured;
        
        var result = await _userManager.SetTwoFactorEnabledAsync(user, true);
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to enable 2FA for user with email {Email} in database: {Message}", user.Email, result.Errors);
            return Errors.UnexpectedError;
        }
        
        return new ConfigureTotpResponse(true);
    }
    
    private async Task<ErrorOr<string>> GenerateTotpSecret()
    {
        byte[] randomBytes = new byte[20];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        
        return randomBytes.ToBase32();
    }
}
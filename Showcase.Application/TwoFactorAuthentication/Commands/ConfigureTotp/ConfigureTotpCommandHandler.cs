using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;
using OtpNet;
using Showcase.Utilities;

namespace Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;

public class ConfigureTotpCommandHandler : IRequestHandler<ConfigureTotpCommand, ErrorOr<ConfigureTotpResponse>>
{
    private readonly ILogger<ConfigureTotpCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    
    public ConfigureTotpCommandHandler(ILogger<ConfigureTotpCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context; 
    }
    
    public async Task<ErrorOr<ConfigureTotpResponse>> Handle(ConfigureTotpCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _context.Users.FindAsync(request.UserId);
        if (user is null)
            return Errors.Authentication.InvalidCredentials;
        
        var totpSecret = await _context.UserTotpSecrets.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);
        if (totpSecret is not null)
            return Errors.TwoFactorAuthentication.TotpAlreadyConfigured;

        string secret = await GenerateTotpSecret();
        var uriString = new OtpUri(OtpType.Totp, secret, user.Email, EnvironmentReader.TwoFactorAuthentication.Issuer).ToString();

        var newTotpSecret = new UserTotpSecret
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TotpSecret = secret
        };
            
        var result = await AddUserTotpSecretAsync(newTotpSecret, cancellationToken);
        if (result.IsError)
        {
            return result.Errors;
        }
        
        return new ConfigureTotpResponse(uriString, secret);
    }
    
    private Task <string> GenerateTotpSecret()
    {
        var key = KeyGeneration.GenerateRandomKey(20);
        var base32String = Base32Encoding.ToString(key);

        return Task.FromResult(base32String);
    }
    
    private async Task<ErrorOr<Unit>> AddUserTotpSecretAsync(UserTotpSecret newTotpSecret, CancellationToken cancellationToken)
    {
        try
        {
            await _context.UserTotpSecrets.AddAsync(newTotpSecret, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new Unit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save UserTotpSecret to database: {Message}", ex.Message);
            return Errors.UnexpectedError;
        }
    }
}
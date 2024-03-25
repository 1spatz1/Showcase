using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.TwoFactorAuthentication.Commands.EnableTotp;

public class EnableTotpCommandHandler : IRequestHandler<EnableTotpCommand, ErrorOr<EnableTotpResponse>>
{
    private readonly ILogger<EnableTotpCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    
    public EnableTotpCommandHandler(ILogger<EnableTotpCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context; 
    }

    public async Task<ErrorOr<EnableTotpResponse>> Handle(EnableTotpCommand request,
        CancellationToken cancellationToken)
    {
        // Get user
        ApplicationUser? findByGuidAsync = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (findByGuidAsync is null)
            return Errors.Authentication.InvalidCredentials;
        
        if (findByGuidAsync.TwoFactorEnabled)
            return Errors.TwoFactorAuthentication.TotpAlreadyConfigured;
        
        findByGuidAsync.TwoFactorEnabled = true;
        if(await SaveToDb(findByGuidAsync, cancellationToken) == false)
            return Errors.UnexpectedError;
        
        return new EnableTotpResponse(true);
    }
    
    private async Task<ErrorOr<Boolean>> SaveToDb(ApplicationUser user, CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to enable 2FA for user with email {Email} in database: {Message}", user.Email, ex.Message);
            return false;
        }
    }
}
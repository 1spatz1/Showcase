using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;
using OtpNet;

namespace Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;

public class VerifyTotpQueryHandler : IRequestHandler<VerifyTotpQuery, ErrorOr<VerifyTotpResponse>>
{
    private readonly ILogger<VerifyTotpQueryHandler> _logger;
    private readonly IApplicationDbContext _context;
    
    public VerifyTotpQueryHandler(ILogger<VerifyTotpQueryHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context; 
    }
    
    public async Task<ErrorOr<VerifyTotpResponse>> Handle(VerifyTotpQuery request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _context.Users.FindAsync(request.UserId);
        if (user is null)
            return Errors.Authentication.InvalidCredentials;
        
        var userTotpSecret = await _context.UserTotpSecrets.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);
        if (userTotpSecret is null)
            return Errors.TwoFactorAuthentication.TotpNotConfigured;
        
        var SecretBytes = Base32Encoding.ToBytes(userTotpSecret.TotpSecret);
        var totp = new Totp(SecretBytes);
        
        long timeWindowUsed;
        var veritfyTotpResult = totp.VerifyTotp(request.Token.Trim(), out timeWindowUsed, VerificationWindow.RfcSpecifiedNetworkDelay);
        
        return new VerifyTotpResponse(veritfyTotpResult);
    }
}
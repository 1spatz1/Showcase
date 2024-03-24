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
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<EnableTotpCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    
    public EnableTotpCommandHandler(ILogger<EnableTotpCommandHandler> logger, IApplicationDbContext context, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _context = context; 
        _mapper = mapper;
        _mediator = mediator;
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
        
        VerifyTotpQuery verifyTotpQuery = _mapper.Map<VerifyTotpQuery>(request);
        ErrorOr<VerifyTotpResponse> verifyTotpResponse = await _mediator.Send(verifyTotpQuery);
        
        if (verifyTotpResponse.IsError)
            return verifyTotpResponse.Errors;
        
        if (verifyTotpResponse.Value.Success == false)
            return Errors.TwoFactorAuthentication.TotpFailure;
        
        findByGuidAsync.TwoFactorEnabled = true;
        
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to enable 2FA for user with email {Email} in database: {Message}", findByGuidAsync.Email, ex.Message);
            return Errors.UnexpectedError;
        }
        
        return new EnableTotpResponse(true);
    }
}
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Showcase.Application.Authentication.Common;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;
using Showcase.Utilities;

namespace Showcase.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResponse>>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginQueryHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService,
        IDateTimeProvider dateTimeProvider, IMediator mediator, IMapper mapper)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<ErrorOr<AuthenticationResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        
        // Check if User with Email exists
        ApplicationUser? findByEmailAsync = await _userManager.FindByEmailAsync(request.Email);
        if (findByEmailAsync is null)
            return Errors.Authentication.InvalidCredentials;

        // Check if the user is locked out
        if (await _userManager.IsLockedOutAsync(findByEmailAsync))
            return Errors.Authentication.UserLockedOut;

        // Check if Password is correct
        var result = await _userManager.CheckPasswordAsync(findByEmailAsync, request.Password);
        if (!result)
        {
            findByEmailAsync.AccessFailedCount++;
            await _userManager.UpdateAsync(findByEmailAsync);
            if (!(findByEmailAsync.AccessFailedCount >= EnvironmentReader.Authentication.MaxFailedAttempts))
                return Errors.Authentication.InvalidCredentials;
            await _userManager.SetLockoutEndDateAsync(findByEmailAsync,
                _dateTimeProvider.UtcNow.AddMinutes((int)EnvironmentReader.Authentication.LockoutMinutes!));
            findByEmailAsync.AccessFailedCount = 0;
            await _userManager.UpdateAsync(findByEmailAsync);
            return Errors.Authentication.UserLockedOut;
        }
            
        
        if (findByEmailAsync.TwoFactorEnabled)
        {
            VerifyTotpQuery verifyTotpQuery = new VerifyTotpQuery(request.Token, findByEmailAsync.Id);
            ErrorOr<VerifyTotpResponse> verifyTotpResponse = await _mediator.Send(verifyTotpQuery);
            
            if (verifyTotpResponse.IsError)
                return verifyTotpResponse.Errors;
        
            if (verifyTotpResponse.Value.Success == false)
                return Errors.TwoFactorAuthentication.TotpFailure;
        }

        string token = await _jwtTokenService.GenerateUserTokenAsync(findByEmailAsync);

        return new AuthenticationResponse(findByEmailAsync.Id, findByEmailAsync.UserName, token,
            _dateTimeProvider.UtcNow.AddMinutes((int)EnvironmentReader.Jwt.ExpiryMinutes!));
    }
}
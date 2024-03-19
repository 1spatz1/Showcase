using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Showcase.Application.Authentication.Common;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;
using Showcase.Utilities;

namespace Showcase.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResponse>>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginQueryHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService,
        IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
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
        bool result = await _userManager.CheckPasswordAsync(findByEmailAsync, request.Password);
        if (!result)
            return Errors.Authentication.InvalidCredentials;

        string token = await _jwtTokenService.GenerateUserTokenAsync(findByEmailAsync);

        return new AuthenticationResponse(findByEmailAsync.Id, findByEmailAsync.UserName, token,
            _dateTimeProvider.UtcNow.AddMinutes((int)EnvironmentReader.Jwt.ExpiryMinutes!));
    }
}
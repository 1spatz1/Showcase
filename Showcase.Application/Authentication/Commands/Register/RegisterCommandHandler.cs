using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Authentication.Common;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;
using Showcase.Domain.Identity;
using Showcase.Utilities;

namespace Showcase.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResponse>>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService,
        IDateTimeProvider dateTimeProvider, ILogger<RegisterCommandHandler> logger)
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
    }

    public async Task<ErrorOr<AuthenticationResponse>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        ApplicationUser user = new()
        {
            UserName = request.Username.ToLower(),
            Email = request.Email
        };
        
        // Check if Email is taken
        if (await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
            return Errors.Authentication.EmailTaken;
        
        // Check if UserName is taken
        if (await _userManager.Users.AnyAsync(x => x.UserName == request.Username.ToLower(), cancellationToken))        
            return Errors.Authentication.UsernameTaken;

        // Create User
        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
        
        // If something went wrong log and return error
        if (!result.Succeeded)
        {
            _logger.LogError("Something went wrong whilst trying to register a new user: {Message}", result.Errors.First().Description);
            return Error.Custom(code: "Register.IdentityFailure", description: result.Errors.First().Description, type: 400);
        }

        await _userManager.AddToRoleAsync(user, IdentityNames.Roles.Member);
        string token = await _jwtTokenService.GenerateUserTokenAsync(user);
        
        // Log username when new user registered
        _logger.LogInformation("New user created: {Username}", user.NormalizedUserName);

        return new AuthenticationResponse(user.Id, user.UserName, token,
            _dateTimeProvider.UtcNow.AddMinutes((int)EnvironmentReader.Jwt.ExpiryMinutes!));
    }
}
using ErrorOr;
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
        // Check if User with Email exists
        ApplicationUser? findByEmailAsync = await _userManager.FindByEmailAsync(request.Email);
        if (findByEmailAsync is null)
            return Errors.Authentication.InvalidCredentials;
        
        
    }
}
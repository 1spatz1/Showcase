using ErrorOr;
using MediatR;
using Showcase.Application.Authentication.Common;

namespace Showcase.Application.Authentication.Commands.Register;

public record RegisterCommand
(
    string Email, 
    string Username, 
    string Password
) : IRequest<ErrorOr<AuthenticationResponse>>;
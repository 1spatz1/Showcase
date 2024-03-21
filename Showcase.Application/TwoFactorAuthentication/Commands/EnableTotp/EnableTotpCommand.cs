using ErrorOr;
using MediatR;

namespace Showcase.Application.TwoFactorAuthentication.Commands.EnableTotp;

public record EnableTotpCommand
(
    Guid UserId,
    string Username,
    string Token
) : IRequest<ErrorOr<EnableTotpResponse>>;
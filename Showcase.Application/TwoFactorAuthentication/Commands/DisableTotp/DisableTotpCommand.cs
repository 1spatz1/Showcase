using ErrorOr;
using MediatR;

namespace Showcase.Application.TwoFactorAuthentication.Commands.DisableTotp;

public record DisableTotpCommand
(
    Guid UserId,
    string Username
) : IRequest<ErrorOr<DisableTotpResponse>>;
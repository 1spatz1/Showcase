using ErrorOr;
using MediatR;

namespace Showcase.Application.TwoFactorAuthentication.Commands.DisableTotp;

public record DisableTotpCommand
(
    Guid UserId
) : IRequest<ErrorOr<DisableTotpResponse>>;
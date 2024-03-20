using ErrorOr;
using MediatR;

namespace Showcase.Application.Authentication.Commands.DisableTotp;

public record DisableTotpCommand
(
    Guid UserId,
    string Username
) : IRequest<ErrorOr<DisableTotpResponse>>;
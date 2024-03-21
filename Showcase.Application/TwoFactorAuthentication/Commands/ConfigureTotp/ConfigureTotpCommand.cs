using ErrorOr;
using MediatR;

namespace Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;

public record ConfigureTotpCommand
(
    Guid UserId
) : IRequest<ErrorOr<ConfigureTotpResponse>>;
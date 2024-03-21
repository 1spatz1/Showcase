using ErrorOr;
using MediatR;

namespace Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;

public record ConfigureTotpCommand
(
    Guid UserId,
    string Username
) : IRequest<ErrorOr<ConfigureTotpResponse>>;
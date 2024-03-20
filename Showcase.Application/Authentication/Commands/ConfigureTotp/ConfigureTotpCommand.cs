using ErrorOr;
using MediatR;

namespace Showcase.Application.Authentication.Commands.ConfigureTotp;

public record ConfigureTotpCommand
(
    Guid UserId,
    string Username
) : IRequest<ErrorOr<ConfigureTotpResponse>>;
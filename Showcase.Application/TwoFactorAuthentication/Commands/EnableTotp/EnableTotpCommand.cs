using ErrorOr;
using MediatR;

namespace Showcase.Application.TwoFactorAuthentication.Commands.EnableTotp;

public record EnableTotpCommand
(
    string Token,
    Guid UserId
) : IRequest<ErrorOr<EnableTotpResponse>>;
using ErrorOr;
using MediatR;

namespace Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;

public record VerifyTotpQuery
(
    Guid UserId,
    string Username,
    string Token
) : IRequest<ErrorOr<VerifyTotpResponse>>;
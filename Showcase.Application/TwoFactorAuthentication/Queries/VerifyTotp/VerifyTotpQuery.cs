using ErrorOr;
using MediatR;

namespace Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;

public record VerifyTotpQuery
(
    string Token,
    Guid UserId
) : IRequest<ErrorOr<VerifyTotpResponse>>;
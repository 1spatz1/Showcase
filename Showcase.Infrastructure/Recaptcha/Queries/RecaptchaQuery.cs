using ErrorOr;
using MediatR;

namespace Showcase.Infrastructure.Recaptcha.Queries;

public record RecaptchaQuery
(
    string RecaptchaToken
) : IRequest<ErrorOr<RecaptchaResponse>>;
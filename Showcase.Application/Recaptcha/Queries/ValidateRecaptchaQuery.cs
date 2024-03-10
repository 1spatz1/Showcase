using ErrorOr;
using MediatR;

namespace Showcase.Infrastructure.Recaptcha.Queries;

public record ValidateRecaptchaQuery
(
    string RecaptchaToken
) : IRequest<ErrorOr<ValidateRecaptchaResponse>>;
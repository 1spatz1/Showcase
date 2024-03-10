using ErrorOr;
using MediatR;

namespace Showcase.Infrastructure.Email.Commands;

public record SendEmailCommand
(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string Subject,
    string Message,
    string RecaptchaToken
) : IRequest<ErrorOr<SendEmailResponse>>;
using ErrorOr;
using MediatR;
using Showcase.Application.Contact.Common;

namespace Showcase.Application.Contact.Commands;

public record ContactCommand
(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string Subject,
    string Message,
    string RecaptchaToken
) : IRequest<ErrorOr<ContactResponse>>;
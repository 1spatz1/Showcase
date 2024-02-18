using FluentValidation;
using FluentValidation.Validators;

namespace Showcase.Application.Contact.Commands;

public class ContactCommandValidator : AbstractValidator<ContactCommand>
{
    // string FirstName,
    // string LastName,
    // string PhoneNumber,
    // string Email,
    // string Subject,
    // string Message,
    // string RecaptchaToken
    public ContactCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Subject must be less then 50 characters long.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Subject must be less then 50 characters long.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"\d{10}");
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Subject)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Subject must be less then 600 characters long.");
        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(600)
            .WithMessage("Message must be less then 600 characters long.");
        RuleFor(x => x.RecaptchaToken)
            .NotEmpty();
    }
}
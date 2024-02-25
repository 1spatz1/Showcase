using FluentValidation;
using FluentValidation.Validators;

namespace Showcase.Application.Contact.Commands;

public class ContactCommandValidator : AbstractValidator<ContactCommand>
{
    public ContactCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Subject must be less then 60 characters long.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Subject must be less then 60 characters long.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)")
            .WithMessage("Subject must be a phone number.")
            .MaximumLength(20)
            .WithMessage("Subject must be less then 20 characters long.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Subject must be an email address.")
            .MaximumLength(80)
            .WithMessage("Subject must be less then 80 characters long.");
        RuleFor(x => x.Subject)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Subject must be less then 200 characters long.");
        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(2000)
            .WithMessage("Message must be less then 2000 characters long.");
        RuleFor(x => x.RecaptchaToken)
            .NotEmpty();
    }
}
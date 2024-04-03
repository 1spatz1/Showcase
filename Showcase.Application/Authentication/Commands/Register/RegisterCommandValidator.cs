using FluentValidation;

namespace Showcase.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(80)
            .WithMessage("Email must be less than 80 characters long.");
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(30)
            .WithMessage("Username must be less than 30 characters long.");
        RuleFor(x => x.Password)
            .MaximumLength(128)
            .WithMessage("Password must be less than 128 characters long.")
            /*// Must have digit
            .Matches(@"[0-9]+")
            .WithMessage("Password must contain at least one digit.")
            // Must have lowercase
            .Matches(@"[a-z]+")
            .WithMessage("Password must contain at least one lowercase letter.")
            // Must have uppercase
            .Matches(@"[A-Z]+")
            .WithMessage("Password must contain at least one uppercase letter.")
            // Must have non-alphanumeric
            .Matches(@"\W+")
            .WithMessage("Password must contain at least one non-alphanumeric character.")*/
            // Must be at least 14 characters
            .MinimumLength(14)
            .WithMessage("Password must be at least 14 characters long.");
        RuleFor(x => x.ConfirmPassword)
            .MaximumLength(128)
            .WithMessage("ConfirmPassword must be less than 128 characters long.")
            .MinimumLength(14)
            .WithMessage("ConfirmPassword must be at least 14 characters long.");
    }
}
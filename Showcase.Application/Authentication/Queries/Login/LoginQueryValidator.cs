using FluentValidation;

namespace Showcase.Application.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            // Maximum length of 80
            .MaximumLength(80)
            .WithMessage("Email must be less than 80 characters long.");
        RuleFor(x => x.Password)
            .MaximumLength(80)
            .NotEmpty();
        RuleFor(x => x.Token)
            .MaximumLength(8);
    }
}
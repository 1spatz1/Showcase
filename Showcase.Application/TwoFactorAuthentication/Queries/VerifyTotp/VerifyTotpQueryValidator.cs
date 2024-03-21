using FluentValidation;

namespace Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;

public class VerifyTotpQueryValidator : AbstractValidator<VerifyTotpQuery>
{
    public VerifyTotpQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.");
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username must not be empty.");
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token must not be empty.");
    }
}
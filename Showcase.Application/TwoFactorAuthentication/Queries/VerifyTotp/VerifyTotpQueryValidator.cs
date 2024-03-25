using FluentValidation;

namespace Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;

public class VerifyTotpQueryValidator : AbstractValidator<VerifyTotpQuery>
{
    public VerifyTotpQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.")
            .Must(guid => Guid.TryParse(guid.ToString(), out _))
            .WithMessage("Invalid UserId format");
        RuleFor(x => x.Token)
            .MaximumLength(8);
    }
}
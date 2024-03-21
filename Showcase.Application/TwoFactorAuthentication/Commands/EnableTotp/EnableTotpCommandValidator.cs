using FluentValidation;
using Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;

namespace Showcase.Application.TwoFactorAuthentication.Commands.EnableTotp;

public class EnableTotpCommandValidator : AbstractValidator<EnableTotpCommand>
{
    public EnableTotpCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.");
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token must not be empty.")
            .MaximumLength(8);
    }
}
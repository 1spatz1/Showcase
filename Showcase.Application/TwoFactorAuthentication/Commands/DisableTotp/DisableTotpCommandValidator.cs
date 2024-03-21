using FluentValidation;

namespace Showcase.Application.TwoFactorAuthentication.Commands.DisableTotp;

public class DisableTotpCommandValidator : AbstractValidator<DisableTotpCommand>
{
    public DisableTotpCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.");
    }
}